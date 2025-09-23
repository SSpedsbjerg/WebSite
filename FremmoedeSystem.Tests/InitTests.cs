namespace FremmoedeSystem.Tests;

public class InitTests {

    private const string certPath = "/run/secrets/simonspedsbjerg.dk.pfx";

    [Fact]
    public void VerifyEncryptionString() {
        Assert.Equivalent(new FremmødeSystem.Structs.StartupConfiguration().CertificationPath, certPath);
    }
}
