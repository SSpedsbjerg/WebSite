namespace FremmødeSystem.Structs {
    public struct StartupConfiguration {
        private const string certPath = "/run/secrets/simonspedsbjerg.dk.pfx";
        public string CertificationPath { get { return certPath; } }
    }
}
