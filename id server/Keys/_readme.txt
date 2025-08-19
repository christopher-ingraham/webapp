To create and configure a new Signing Certificate:

1.	open powershell

2.	enter the following command
		./New-SigninCertificate.ps1 -DNSName <domain> -Password <password>
	where <domain> is your host domain (es. dawi.host.net) and <password> is at your choice

3.	open app.<environment>.settings and configure the following section as:

  "SigningCredential": {
    "KeyFilePath": "Keys\\<domain>.pfx",
    "KeyFilePassword": "<password>"
  },