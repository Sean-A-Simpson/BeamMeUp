namespace BeamMeUp
{
    public class Platform
    {
        private Platform(string value) { Value = value; }

        public string Value { get; private set; }

        public static Platform iOS { get { return new Platform("iOS"); } }
        public static Platform Google { get { return new Platform("Google"); } }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(Platform platform) { return platform.Value; }
    }
}
