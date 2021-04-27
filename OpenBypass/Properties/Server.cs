using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Text.RegularExpressions;

namespace ActivationServer
{
    class ActivationServer
    {
        public void StartServer()
        {
            HttpListener actServer = new HttpListener();
            actServer.Prefixes.Add("http://localhost:5000/"); // add prefix "http://localhost:5000/"
            actServer.Start();

            while (true)
            {
                HttpListenerContext context = actServer.GetContext();
                HttpListenerResponse response = context.Response;
                HttpListenerRequest request = context.Request;

                string data = GetRequestPostData(request);

                if (string.IsNullOrEmpty(data))
                {
                    byte[] noBuff = Encoding.UTF8.GetBytes("<pre>OpenBypass </pre>");

                    response.ContentLength64 = noBuff.Length;
                    response.Headers.Add("Content-type", "text/html; charset=UTF-8");
                    response.Headers.Add("Server", "iHacktivate\r\n\r\n");
                    Stream ns = response.OutputStream;
                    ns.Write(noBuff, 0, noBuff.Length);
                    context.Response.Close();
                    continue;
                }

                Dictionary<string, string> activationData = GetActivationData(data);

                string fpkd = FairPlayKeyData("");
                string atc = AccountTokenCertificate();
                string dc = DeviceCertificate();
                string udc = UniqueDeviceCertificate();
                string at = AccountToken(activationData);
                string ats = SignData(at);
                at = Convert.ToBase64String(Encoding.UTF8.GetBytes(at));

                string msg = ActivationData(fpkd, atc, dc, ats, at, udc);

                string subDir = @"Data//" + activationData["serialNumber"] + "//";
                bool exists = Directory.Exists(subDir);

                if (!exists)
                    Directory.CreateDirectory(subDir);

                File.WriteAllText(subDir + "activation_record.plist", msg);

                byte[] buffer = Encoding.UTF8.GetBytes(msg);

                response.ContentLength64 = buffer.Length;
                response.Headers.Add("Content-type", "application/xml");
                response.Headers.Add("Server", "iHacktivate\r\n\r\n");
                Stream st = response.OutputStream;
                st.Write(buffer, 0, buffer.Length);

                context.Response.Close();

                if (!string.IsNullOrEmpty(msg))
                {
                    actServer.Stop();
                    break;
                }
            }
        }

        public static string GetRequestPostData(HttpListenerRequest request)
        {
            if (!request.HasEntityBody || !request.ContentType.ToLowerInvariant().StartsWith("multipart/form-data"))
            {
                return null;
            }
            using (Stream body = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(body, request.ContentEncoding))
                {
                    var contentTypeRegex = new Regex("^multipart\\/form-data;\\s*boundary=(.*)$", RegexOptions.IgnoreCase);
                    var boundaryRegex = new Regex("boundary=(.*)$", RegexOptions.IgnoreCase);
                    var bodyStream = reader.ReadToEnd();

                    if (contentTypeRegex.IsMatch(request.ContentType))
                    {
                        var boundary = boundaryRegex.Match(request.ContentType).Groups[1].Value;

                        bodyStream = bodyStream.Replace("Content-Disposition: form-data; name=\"activation-info\"", "");
                        bodyStream = bodyStream.Replace(boundary, "");
                        bodyStream = bodyStream.Replace("--", "");
                        bodyStream = bodyStream.Replace("\r\n", "");
                        bodyStream = bodyStream.Replace("<data>\n", "<data>");
                        bodyStream = bodyStream.Replace("</data>\n", "</data>");
                        bodyStream = bodyStream.Replace("\t", "");

                        return bodyStream;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public Dictionary<string, string> GetActivationData(string data)
        {
            Dictionary<string, string> activationData = new Dictionary<string, string>();
            string xmlData = "";

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(data);
                foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes[1])
                {
                    xmlData = xmlNode.InnerText;
                    xmlData = Encoding.Default.GetString(Convert.FromBase64String(xmlData));
                }
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("Wrong!");
            }

            xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(xmlData);
                XmlNodeList dict = xmlDoc.GetElementsByTagName("dict");

                foreach (XmlNode xmlNode in dict)
                {
                    var nodes = xmlNode.ChildNodes;
                    for (int i = 0; i < nodes.Count - 1; i = i + 2)
                    {
                        switch (nodes.Item(i).InnerText)
                        {
                            case "ActivationRandomness":
                                activationData.Add("activationRandomness", nodes.Item(i + 1).InnerText);
                                break;
                            case "ActivationState":
                                activationData.Add("activationState", nodes.Item(i + 1).InnerText);
                                break;
                            case "BasebandSerialNumber":
                                activationData.Add("basebandSerialNumber", nodes.Item(i + 1).InnerText);
                                break;
                            case "DeviceCertRequest":
                                activationData.Add("deviceCertRequest", Encoding.Default.GetString(Convert.FromBase64String(nodes.Item(i + 1).InnerText)));
                                break;
                            case "DeviceClass":
                                activationData.Add("deviceClass", nodes.Item(i + 1).InnerText);
                                break;
                            case "IntegratedCircuitCardIdentity":
                                activationData.Add("integratedCircuitCardIdentity", nodes.Item(i + 1).InnerText);
                                break;
                            case "InternationalMobileEquipmentIdentity":
                                activationData.Add("internationalMobileEquipmentIdentity", nodes.Item(i + 1).InnerText);
                                break;
                            case "InternationalMobileSubscriberIdentity":
                                activationData.Add("internationalMobileSubscriberIdentity", nodes.Item(i + 1).InnerText);
                                break;
                            case "MobileEquipmentIdentifier":
                                activationData.Add("mobileEquipmentIdentifier", nodes.Item(i + 1).InnerText);
                                break;
                            case "ProductType":
                                activationData.Add("productType", nodes.Item(i + 1).InnerText);
                                break;
                            case "ProductVersion":
                                activationData.Add("productVersion", nodes.Item(i + 1).InnerText);
                                break;
                            case "SerialNumber":
                                activationData.Add("serialNumber", nodes.Item(i + 1).InnerText);
                                break;
                            case "UniqueChipID":
                                activationData.Add("uniqueChipID", nodes.Item(i + 1).InnerText);
                                break;
                            case "UniqueDeviceID":
                                activationData.Add("uniqueDeviceID", nodes.Item(i + 1).InnerText);
                                break;
                            case "WildcardTicket":
                                activationData.Add("wildcardTicket", nodes.Item(i + 1).InnerText);
                                break;
                        }
                    }
                }
            }

            catch (FileNotFoundException)
            {
                Debug.WriteLine("Wrong!");
            }

            return activationData;
        }

        private string FairPlayKeyData(string device)
        {
            string fpkd = "LS0tLS1CRUdJTiBDT05UQUlORVItLS0tLQpBQUVBQVM3UldON2k5bHR0ZVhMMzl5dDZ1eThVcnV2SFIrVmkzYmU0UUwyV2hzQXdEZjRGRGlGTzQrMGxRbTNsCnowVUIwQm1laWF3VnNubVczSFFzelI1L2dKVER5M0xJakJsbDJac2VsOXBvc0czRHdMUGczd1RGZjZLelMraGYKUHU0UFRrcE81OVJUOFpaZWtoQ2tCQ2Nmd1FKbGhMZWJnMmEzS0NkcHRTV1phYllNamhxYXB0Sy9KVnhMUm9aSwphcFVkcmpPb3pOMXZPalh0STlUY0I1V0F2ak9kbW15MktsRjI1U3o4L0c3U1RmSmM0WDRQTXE2bUM4cmsvZ0xqClRRVTNsZUs3QnJJdEZ0RFdVSnhTaVVuN2xiZVRYRWliVnNoVUMrUms0VGhnK0dURThnVjNOY3BxbytEL3pWSTEKMkE1MDI3ZkdtYjBzWUV3cno0Mi9KQmRQZUVjbjVpd1RjS2h2dVZyRkJBSnFoQXFEZGN2Z091TjNtY3ZMUk0zZAo4QXNLZTNkZDI5cjVBTEUrRFRnSDVNNE4wcG16cmhyY2RzRUlmZ0Y1TW1Femp0bnY0d2kya3FxMUtMa3Q2ZDFpCmxZQVRqaktHRzh4T290alovcXRXSlZnSXVpdGlXWmJQUE5YcDltbGoxK1JqMngya25qZHNJYWhwMjA3VUpOd28KcGxFSEw1TkR0VTQwcU9IRDlYTE5pcmtKMXBxMFczcXF5a0U5UHRqV1R3Sm9BZ1c4cjd0T08xNnF6SVRYVFRnSAprYllQMmwva2txeTJpWGl2L2lXb21FNVZyK1pVMjZGUC9JSnVaRC9FaVhGRWZTQ0RMdGdTQlVJQUNFMTF6UFRVCkt0NEFqVzhkZ0Q3amUzcGg1VWVFYThNK1JmSHJxWDJsajRPRGFXcHI0Z1BVZmxWRG5UV3VuUkF4MHp5M1Nyd3IKZjRvNTlNeWcyeU9zanlQQTNDNFJ4QnU3eE5CSk5hYkJOZFEvNDBsRzNLQURFNjZoQ25oMEt6c2Q5MVZmSExOSQpLOG9OVlNsZjg3Y2Vtdk5CaEZYK3NRd0YwVXltR0gvS3U1SGY0ZXJqRFgvbVB0RWRIbmJtNVk5VUNUUTkweDhjCjJYYnVwakRpczlRQitsUEwyWU1JUFBkTlI1RUROM2ZMN24rV1IxMCtsQTBBeUVySUJKLzhFUHdlOFlacFd0RnIKRFNQMzlFQllYdmMxcklreno5VU92SHlaeE5EY29WNWdVOWNyekF2c3lFZUpBd2dieTExRG15UEtSVWVTeG9VSAprbnpXRitSUnp2UlJlTmFpaUtWOERZbWJIQisyb0VrbmZ1ZVVNTGhPSFRGQkJ3WFZHSjBPbnR3NU8vSHlMZTR1CkJoMjJudXlxazdWTEVEaGhEL3p4QWVYZnZrR3dNT3IxeGJOeXMyenpmMlk3M0VQQi95czFOWXArQU5QSWhheEUKZEVkN1F5VzhWY3FySm90aVo2azZoU2VLbHRrOUZrcWxPalcrYmJuSzBWMURhRE9oQVU5ZTl2REVtV0lWTHgxdwplQjk3NDk5UFNOVlZxTHlJYUJWcUQ3TmtlWkZTd3kwUDlQdzVXNDh1cFZ4bnRsVWtTL0dpdk1lcVhzWjNNUWtnCmsyMzluZFJOemxCZkVZc0NaKzdsMm9HTnRWOEppbVpFKzMwQnlUS01XUXRkcmdDNXhxdmU5WFdtYTdVdmZ3NHAKSXFaYmw5SDR4eHJlNkVBVTFFNlpnVElrbDBmdTVjSmw2eVVpUHhkSFptdHJrWEpzN25Wbm83Z0RRbzcrRUYzdgpzQ0lMT0FoSHhMUmtvL1ZXNGtlenZWM0JmaXZtNVZZdGdtaG52WHF0VGVvcVVwZlFJZndvaG81UWZwYTQyVTdoCk5IeWxTVndlMDg3SlNncG5qV0VRMHlHYXpJZCtZYlozVHEwbDBlRHNFbkYzYnFnbUZwR2N6L3BJSVFBV0xER3oKZmpzdTN4dFNILzZaTGVwK290Y3krcmlsSXNKVFRiUHVNS01BWk5ZNGVacWtJTHBQCi0tLS0tRU5EIENPTlRBSU5FUi0tLS0tCg==";
            return fpkd;
        }

        //GSM
        private string UniqueDeviceCertificate()
        {
            string udc = "LS0tLS1CRUdJTiBDRVJUSUZJQ0FURS0tLS0tCk1JSURBRENDQXFXZ0F3SUJBZ0lHQVd3bzRhOXJNQW9HQ0NxR1NNNDlCQU1DTUVVeEV6QVJCZ05WQkFnTUNrTmgKYkdsbWIzSnVhV0V4RXpBUkJnTlZCQW9NQ2tGd2NHeGxJRWx1WXk0eEdUQVhCZ05WQkFNTUVFWkVVa1JETFZWRApVbFF0VTFWQ1EwRXdIaGNOTVRrd056STFNVEV4TmpFMFdoY05NVGt3T0RBeE1URXlOakUwV2pCdU1STXdFUVlEClZRUUlEQXBEWVd4cFptOXlibWxoTVJNd0VRWURWUVFLREFwQmNIQnNaU0JKYm1NdU1SNHdIQVlEVlFRTERCVjEKWTNKMElFeGxZV1lnUTJWeWRHbG1hV05oZEdVeElqQWdCZ05WQkFNTUdUQXdNREE0TURFd0xUQXdNVFkzTURsRgpNVGd6UWpoR01qWXdXVEFUQmdjcWhrak9QUUlCQmdncWhrak9QUU1CQndOQ0FBUm1ROUE4cUJ4bCtEdmdhU0NICkFCZWdtSi94TzE3QUx2UVNPWVU5NWtPR0FrN2MvbWpRaFIvQTVKR3NvZDI1WS9KSUU3WGxWQ3JkTkhPQmxFNEsKRDBSZG80SUJWakNDQVZJd0RBWURWUjBUQVFIL0JBSXdBREFPQmdOVkhROEJBZjhFQkFNQ0JQQXdnZmNHQ1NxRwpTSWIzWTJRS0FRU0I2VEdCNXYrRW1xR1NVQTB3Q3hZRVEwaEpVQUlEQUlBUS80U3FqWkpFRVRBUEZnUkZRMGxFCkFnY1djSjRZTzQ4bS80YVR0Y0pqR3pBWkZnUmliV0ZqQkJGa01EbzRNVG8zWVRvellqbzBZem95WnYrR3k3WEsKYVJrd0Z4WUVhVzFsYVFRUE16VTJOVFUwTURnM016a3dORGM0LzRlYnlkeHRGakFVRmdSemNtNXRCQXhFV0ROVwpSREpCVFVoSE4xRC9oNnVSMG1ReU1EQVdCSFZrYVdRRUtHSmpPVFk0TkRnME9XSXdZbUkzTnpGbE1HSXlaalkwCk5HWTNZVGd5TlRoaU56STJZVE00WlRiL2g3dTF3bU1iTUJrV0JIZHRZV01FRVdRd09qZ3hPamRoT2pOaU9qUmoKT2pKbE1CSUdDU3FHU0liM1kyUUtBZ1FGTUFNQ0FRQXdKQVlKS29aSWh2ZGpaQWdIQkJjd0ZiK0tlQVlFQkRFeQpMalMvaW5zSEJBVXhOa2MzTnpBS0JnZ3Foa2pPUFFRREFnTkpBREJHQWlFQWdXdjVmd1JGWDJ3eHg5NmpCN0RzCld0K1NkN28zcHNmWWs0ZUpleWp5dFJFQ0lRQ1lCTERFUGVycTJFZWw0TWZLNWJUYlFLbG9jUXB4SkV3V3NiY0cKaWZoM25nPT0KLS0tLS1FTkQgQ0VSVElGSUNBVEUtLS0tLXVrTXRIOVJkU1F2SHpCeDdGaUJHcjcvS2NtbHhYL1h3b1dlV25XYjZJUk09Ci0tLS0tQkVHSU4gQ0VSVElGSUNBVEUtLS0tLQpNSUlDRnpDQ0FaeWdBd0lCQWdJSU9jVXFROElDL2hzd0NnWUlLb1pJemowRUF3SXdRREVVTUJJR0ExVUVBd3dMClUwVlFJRkp2YjNRZ1EwRXhFekFSQmdOVkJBb01Da0Z3Y0d4bElFbHVZeTR4RXpBUkJnTlZCQWdNQ2tOaGJHbG0KYjNKdWFXRXdIaGNOTVRZd05ESTFNak0wTlRRM1doY05Namt3TmpJME1qRTBNekkwV2pCRk1STXdFUVlEVlFRSQpEQXBEWVd4cFptOXlibWxoTVJNd0VRWURWUVFLREFwQmNIQnNaU0JKYm1NdU1Sa3dGd1lEVlFRRERCQkdSRkpFClF5MVZRMUpVTFZOVlFrTkJNRmt3RXdZSEtvWkl6ajBDQVFZSUtvWkl6ajBEQVFjRFFnQUVhRGMyTy9NcnVZdlAKVlBhVWJLUjdSUnpuNjZCMTQvOEtvVU1zRURiN25Ia0dFTVg2ZUMrMGdTdEdIZTRIWU1yTHlXY2FwMXRERlltRQpEeWtHUTN1TTJhTjdNSGt3SFFZRFZSME9CQllFRkxTcU9rT3RHK1YremdvTU9CcTEwaG5MbFRXek1BOEdBMVVkCkV3RUIvd1FGTUFNQkFmOHdId1lEVlIwakJCZ3dGb0FVV08vV3ZzV0NzRlROR0thRXJhTDJlM3M2Zjg4d0RnWUQKVlIwUEFRSC9CQVFEQWdFR01CWUdDU3FHU0liM1kyUUdMQUVCL3dRR0ZnUjFZM0owTUFvR0NDcUdTTTQ5QkFNQwpBMmtBTUdZQ01RRGY1ek5paUtOL0pxbXMxdyszQ0RZa0VTT1BpZUpNcEVrTGU5YTBValdYRUJETDBWRXNxL0NkCkUzYUtYa2M2UjEwQ01RRFM0TWlXaXltWStSeGt2eS9oaWNERFFxSS9CTCtOM0xIcXpKWlV1dzJTeDBhZkRYN0IKNkx5S2src0xxNHVya01ZPQotLS0tLUVORCBDRVJUSUZJQ0FURS0tLS0t";
            return udc;
        }

        private string AccountTokenCertificate()
        {
            string atc = "LS0tLS1CRUdJTiBDRVJUSUZJQ0FURS0tLS0tCk1JSURaekNDQWsrZ0F3SUJBZ0lCQWpBTkJna3Foa2lHOXcwQkFRVUZBREI1TVFzd0NRWURWUVFHRXdKVlV6RVQKTUJFR0ExVUVDaE1LUVhCd2JHVWdTVzVqTGpFbU1DUUdBMVVFQ3hNZFFYQndiR1VnUTJWeWRHbG1hV05oZEdsdgpiaUJCZFhSb2IzSnBkSGt4TFRBckJnTlZCQU1USkVGd2NHeGxJR2xRYUc5dVpTQkRaWEowYVdacFkyRjBhVzl1CklFRjFkR2h2Y21sMGVUQWVGdzB3TnpBME1UWXlNalUxTURKYUZ3MHhOREEwTVRZeU1qVTFNREphTUZzeEN6QUoKQmdOVkJBWVRBbFZUTVJNd0VRWURWUVFLRXdwQmNIQnNaU0JKYm1NdU1SVXdFd1lEVlFRTEV3eEJjSEJzWlNCcApVR2h2Ym1VeElEQWVCZ05WQkFNVEYwRndjR3hsSUdsUWFHOXVaU0JCWTNScGRtRjBhVzl1TUlHZk1BMEdDU3FHClNJYjNEUUVCQVFVQUE0R05BRENCaVFLQmdRREZBWHpSSW1Bcm1vaUhmYlMyb1BjcUFmYkV2MGQxams3R2JuWDcKKzRZVWx5SWZwcnpCVmRsbXoySkhZdjErMDRJekp0TDdjTDk3VUk3ZmswaTBPTVkwYWw4YStKUFFhNFVnNjExVApicUV0K25qQW1Ba2dlM0hYV0RCZEFYRDlNaGtDN1QvOW83N3pPUTFvbGk0Y1VkemxuWVdmem1XMFBkdU94dXZlCkFlWVk0d0lEQVFBQm80R2JNSUdZTUE0R0ExVWREd0VCL3dRRUF3SUhnREFNQmdOVkhSTUJBZjhFQWpBQU1CMEcKQTFVZERnUVdCQlNob05MK3Q3UnovcHNVYXEvTlBYTlBIKy9XbERBZkJnTlZIU01FR0RBV2dCVG5OQ291SXQ0NQpZR3UwbE01M2cyRXZNYUI4TlRBNEJnTlZIUjhFTVRBdk1DMmdLNkFwaGlkb2RIUndPaTh2ZDNkM0xtRndjR3hsCkxtTnZiUzloY0hCc1pXTmhMMmx3YUc5dVpTNWpjbXd3RFFZSktvWklodmNOQVFFRkJRQURnZ0VCQUY5cW1yVU4KZEErRlJPWUdQN3BXY1lUQUsrcEx5T2Y5ek9hRTdhZVZJODg1VjhZL0JLSGhsd0FvK3pFa2lPVTNGYkVQQ1M5Vgp0UzE4WkJjd0QvK2Q1WlFUTUZrbmhjVUp3ZFBxcWpubTlMcVRmSC94NHB3OE9OSFJEenhIZHA5NmdPVjNBNCs4CmFia29BU2ZjWXF2SVJ5cFhuYnVyM2JSUmhUekFzNFZJTFM2alR5Rll5bVplU2V3dEJ1Ym1taWdvMWtDUWlaR2MKNzZjNWZlREF5SGIyYnpFcXR2eDNXcHJsanRTNDZRVDVDUjZZZWxpblpuaW8zMmpBelJZVHh0UzZyM0pzdlpEaQpKMDcrRUhjbWZHZHB4d2dPKzdidFcxcEZhcjBaakY5L2pZS0tuT1lOeXZDcndzemhhZmJTWXd6QUc1RUpvWEZCCjRkK3BpV0hVRGNQeHRjYz0KLS0tLS1FTkQgQ0VSVElGSUNBVEUtLS0tLQo=";
            return atc;
        }

        private string AccountToken(Dictionary<string, string> activationData)
        {
            string at = "{" +
                        (activationData.ContainsKey("internationalMobileEquipmentIdentity") ? "\"InternationalMobileEquipmentIdentity\" = \"" + activationData["internationalMobileEquipmentIdentity"] + "\";" : "") +
                        (activationData.ContainsKey("mobileEquipmentIdentifier") ? "\"MobileEquipmentIdentifier\" = \"" + activationData["mobileEquipmentIdentifier"] + "\";" : "") +
                        "\"SerialNumber\" = \"" + activationData["serialNumber"] + "\";" +
                        (activationData.ContainsKey("internationalMobileSubscriberIdentity") ? "\"InternationalMobileSubscriberIdentity\" = \"" + activationData["internationalMobileSubscriberIdentity"] + "\";" : "") +
                        "\"ProductType\" = \"" + activationData["productType"] + "\";" +
                        "\"UniqueDeviceID\" = \"" + activationData["uniqueDeviceID"] + "\";" +
                        "\"ActivationRandomness\" = \"" + activationData["activationRandomness"] + "\";" +
                        "\"ActivityURL\" = \"https://albert.apple.com/deviceservices/activity\";" +
                        "\"IntegratedCircuitCardIdentity\" = \"" + (activationData.ContainsKey("integratedCircuitCardIdentity") ? activationData["integratedCircuitCardIdentity"] : "") + "\";" +
                        (activationData["deviceClass"] == "iPhone" ? "\"CertificateURL\" = \"https://albert.apple.com/deviceservices/certifyMe\";" : "") +
                        (activationData["deviceClass"] == "iPhone" ? "\"PhoneNumberNotificationURL\" = \"https://albert.apple.com/deviceservices/phoneHome\";" : "") +
                        "\"WildcardTicket\" = \"" + (activationData.ContainsKey("wildcardTicket") ? activationData["wildcardTicket"] : "") + "\";" +
                        "}";
            return at;
        }

        private string DeviceCertificate()
        {
            string dc = "LS0tLS1CRUdJTiBDRVJUSUZJQ0FURS0tLS0tCk1JSUM4ekNDQWx5Z0F3SUJBZ0lLQXdGMC9RUmVmdFZQcmpBTkJna3Foa2lHOXcwQkFRVUZBREJhTVFzd0NRWUQKVlFRR0V3SlZVekVUTUJFR0ExVUVDaE1LUVhCd2JHVWdTVzVqTGpFVk1CTUdBMVVFQ3hNTVFYQndiR1VnYVZCbwpiMjVsTVI4d0hRWURWUVFERXhaQmNIQnNaU0JwVUdodmJtVWdSR1YyYVdObElFTkJNQjRYRFRFNU1EY3lOVEV4Ck1qWXhORm9YRFRJeU1EY3lOVEV4TWpZeE5Gb3dnWU14TFRBckJnTlZCQU1XSkRSR016UTNPVVZETFRReU9VVXQKTkVVd1JDMUJSVGt6TFVRMU1ERkdRVGRGTWtVd1FURUxNQWtHQTFVRUJoTUNWVk14Q3pBSkJnTlZCQWdUQWtOQgpNUkl3RUFZRFZRUUhFd2xEZFhCbGNuUnBibTh4RXpBUkJnTlZCQW9UQ2tGd2NHeGxJRWx1WXk0eER6QU5CZ05WCkJBc1RCbWxRYUc5dVpUQ0JuekFOQmdrcWhraUc5dzBCQVFFRkFBT0JqUUF3Z1lrQ2dZRUFweHhhOFoxdm4rNGkKcFhwK0pPSWpYYVJobkhRRmZFa2FMVFowbmJ4WHM2cStwcDYwN1pyeEJUTG92TkpqRkVRRzZhM1YwbkdDcXZ5VQp4bXhZc3FlanlHczJmbDV1WTdvczh3WStiRjBVS1ZaNHhSMGNWdVN2ZkxySGJhb2Z4anlsRDlMdlZZT21ma0pSCnRTMDdkRUc0cSszRkpXaEFyY3BSRjFpRmsybGQ4WGNDQXdFQUFhT0JsVENCa2pBZkJnTlZIU01FR0RBV2dCU3kKL2lFalJJYVZhbm5WZ1NhT2N4RFlwMHlPZERBZEJnTlZIUTRFRmdRVS9WcWhFTHFaeTJSN3lkMkpvc3pTQ3FSQwpmcEl3REFZRFZSMFRBUUgvQkFJd0FEQU9CZ05WSFE4QkFmOEVCQU1DQmFBd0lBWURWUjBsQVFIL0JCWXdGQVlJCkt3WUJCUVVIQXdFR0NDc0dBUVVGQndNQ01CQUdDaXFHU0liM1kyUUdDZ0lFQWdVQU1BMEdDU3FHU0liM0RRRUIKQlFVQUE0R0JBTndXUU5aZ29ZQ3Y0ZDdQU2xQZ3hUcWYzSFhqUEVpaTlPRzlJZXE3NFlqemhPQVdkK3ltWFlNQgpXNjlwRUdITi9xWEJ4b0JINUhWb2ZuNFEzUUFhOG5OOWlERDRRcDRGdW5YQ1d1alhwUFY4QmJUcnhKcXgwaUZiCnpucTBkZTJOUjUvcUtRbjlpQ0w0VGVmSWhpWllvcnM3d3NWRU83Tm0vd1RTOFAxcUlwOFUKLS0tLS1FTkQgQ0VSVElGSUNBVEUtLS0tLQo=";
            return dc;
        }

        private string ActivationData(string fpkd, string atc, string dc, string ats, string at, string udc)
        {
            string ad =
                "<plist version=\"1.0\">\n" +
                "    <dict>\n" +
                "        <key>iphone-activation</key>\n" +
                "        <dict>\n" +
                "            <key>activation-record</key>\n" +
                "            <dict>\n" +
                "                <key>unbrick</key>\n" +
                "                <true/>\n" +
                "                <key>FairPlayKeyData</key>\n" +
                "                <data>[FairPlayKeyData]</data>\n" +
                "                <key>AccountTokenCertificate</key>\n" +
                "                <data>[AccountTokenCertificate]</data>\n" +
                "                <key>DeviceCertificate</key>\n" +
                "                <data>[DeviceCertificate]</data>\n" +
                "                <key>AccountTokenSignature</key>\n" +
                "                <data>[AccountTokenSignature]</data>\n" +
                "                <key>AccountToken</key>\n" +
                "                <data>[AccountToken]</data>\n" +
                "                <key>LDActivationVersion</key>\n" +
                "                <integer>2</integer>\n" +
                "                <key>UniqueDeviceCertificate</key>\n" +
                "                <data>[UniqueDeviceCertificate]</data>\n" +
                "            </dict>\n" +
                "            <key>show-settings</key>\n" +
                "            <false/>\n" +
                "        </dict>\n" +
                "    </dict>\n" +
                "</plist>";

            ad = ad
                .Replace("[FairPlayKeyData]", fpkd)
                .Replace("[AccountTokenCertificate]", atc)
                .Replace("[DeviceCertificate]", dc)
                .Replace("[AccountTokenSignature]", ats)
                .Replace("[AccountToken]", at)
            .Replace("[UniqueDeviceCertificate]", udc);

            return ad;
        }

        public static string SignData(string message)
        {

            X509Certificate2 certs = new X509Certificate2(@"Data//certs.pfx");
            
            /*
            var pkey = @"Bag Attributes
localKeyID: 14 96 E3 19 1C 05 9A B0 75 F5 63 88 BD 2A 12 FD 05 90 65 3D 
subject=/C=US/O=Apple Inc./OU=Apple iPhone/CN=[TEST] Apple iPhone Activation
issuer=/C=US/O=Apple Inc./OU=Apple Certification Authority/CN=[TEST] Apple iPhone Certification Authority
-----BEGIN CERTIFICATE-----
MIIDdjCCAl6gAwIBAgIBAjANBgkqhkiG9w0BAQUFADCBgDELMAkGA1UEBhMCVVMx
EzARBgNVBAoTCkFwcGxlIEluYy4xJjAkBgNVBAsTHUFwcGxlIENlcnRpZmljYXRp
b24gQXV0aG9yaXR5MTQwMgYDVQQDFCtbVEVTVF0gQXBwbGUgaVBob25lIENlcnRp
ZmljYXRpb24gQXV0aG9yaXR5MB4XDTA3MDMyMTA2MjA1MFoXDTIyMDMxMjA2MjA1
MFowYjELMAkGA1UEBhMCVVMxEzARBgNVBAoTCkFwcGxlIEluYy4xFTATBgNVBAsT
DEFwcGxlIGlQaG9uZTEnMCUGA1UEAxQeW1RFU1RdIEFwcGxlIGlQaG9uZSBBY3Rp
dmF0aW9uMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCzYmXsSN3d7UTU8f77
wm9C0IIJAwCmAeixBwkmWxJl239RFe9PRbOPzk0WHTiEARBXToxx4V7eZxR12kia
TG/wRWVm6Jy1okz0U8HsmGKQsJS+EvKgrFx3FgdzclqXulBOZzBSHvAwTo+ypNPR
+vhmeYeRL6HvTuZBjZQYKeDyzwIDAQABo4GbMIGYMA4GA1UdDwEB/wQEAwIHgDAM
BgNVHRMBAf8EAjAAMB0GA1UdDgQWBBTAbzpKG+1R3Z2jS8BB9moR+auL8TAfBgNV
HSMEGDAWgBRFokypiltKJ16Fpk0FHCdEpYd2FzA4BgNVHR8EMTAvMC2gK6Aphido
dHRwOi8vd3d3LmFwcGxlLmNvbS9hcHBsZWNhL2lwaG9uZS5jcmwwDQYJKoZIhvcN
AQEFBQADggEBAA5PVQCdHkx1KOh5u9tcXMxFk0bTOxQALVuwjC4VPhlgQyVZf3s6
snzPwpbdsNdwWo4oxMwyLPTCQ+PhH7Ox3+yG3UOTRWChUwW6anBDRBGhnt3BcYvL
MM3TFSHgJzA1inaOwiP7RCK9lvaqVb9NEbEOxH/Pho34MN2ASF2eQU7gKZYlzNZK
DUcFh8Rh9rAeGtpIVqPBwVaVs06EghulTdPQXWDP/eV1PJET4uTxYEpYk94/6Ty/
2sn275uesW54noH250wJsxyHJb+Pbj/WA1MeCaNbAEuemPuuE4bkR2oaRPgVyPsw
x72w6IysLXMIr+2y6PVtoeLZRd7VuZiJFnI=
-----END CERTIFICATE-----
Bag Attributes
    localKeyID: 14 96 E3 19 1C 05 9A B0 75 F5 63 88 BD 2A 12 FD 05 90 65 3D 
Key Attributes: <No Attributes>
-----BEGIN PRIVATE KEY-----
MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBALNiZexI3d3tRNTx
/vvCb0LQggkDAKYB6LEHCSZbEmXbf1EV709Fs4/OTRYdOIQBEFdOjHHhXt5nFHXa
SJpMb/BFZWbonLWiTPRTweyYYpCwlL4S8qCsXHcWB3NyWpe6UE5nMFIe8DBOj7Kk
09H6+GZ5h5Evoe9O5kGNlBgp4PLPAgMBAAECgYEAovu/MU0PUKlYhcYN7368ik9a
Jof02eAYJGgJFzFd6O/ioLC2SydAO2OsxG/uPnmc+EZk8r9a6+VqQJ4AozjZ5kX+
raBEF4Bkk1CRIcFb1384S3tuVr3YWP7wiM8DdxD57Kj8rI20Xo4v0oTqCEyyQqwY
GR0xmQMkczFW76wrfmkCQQDdAFs7AHi090rjD5HC5+hOaZIPFMY7m6+mwFGcVDgr
SzdZ8+OG6YNBEF6Jz4fFMNjvY7X+IOlNsv2lNbh/mp6zAkEAz8rXTxrfvQPJsDzs
VhW8wM4YEmy50jMsuE2Ph4/ik7b4NqL3nlipdMXWe2U5fuUPvdBgXKt+u8ypaPje
LeVpdQJAVTKa+Q8AebtP6mMJLVtb6ka2oQvANCCbwawoihzJnp8bkpj8IPmKuR2H
ZJdV3wYqy3bkJTko1+Rl9jfUjZTdEwJBALgOoYc1c8fWihms7V8XZBmYtKPlYPe7
UrpyIVff8MANS3ICCrpdKMUB1Ql6UWKAfeARqrmLQvgQwmL0RsF6u10CQB0x5/lI
2SB7bqerufKWrT+ZUDSwQ9MD9tIAytkNWBXhQEterXbLWlACsn/yvOR0vjHp9qTn
SrO6kBr8l6vySJ4=
-----END PRIVATE KEY-----";*/

            RSA rsaPriv = (RSA)certs.PrivateKey;

            var encoder = new UTF8Encoding();
            byte[] signedBytes;
            byte[] originalData = encoder.GetBytes(message);

            try
            {
                signedBytes = rsaPriv.SignData(originalData, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            }
            catch (CryptographicException e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }

            return Convert.ToBase64String(signedBytes);
        }
    }
}

