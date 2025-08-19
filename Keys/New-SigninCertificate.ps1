Param([string]$Password = '<Password>', [string]$DNSName = '<Domain Name>')
 
$SecurePassword = ConvertTo-SecureString -String $Password `
                                         -AsPlainText `
                                         -Force
 
$CertFileName = "$DNSName.pfx"
 
$NewCert = New-SelfSignedCertificate -CertStoreLocation Cert:\CurrentUser\My `
                                     -DnsName $DNSName
 
Export-PfxCertificate -FilePath $CertFileName `
                      -Password $SecurePassword `
                      -Cert $NewCert
