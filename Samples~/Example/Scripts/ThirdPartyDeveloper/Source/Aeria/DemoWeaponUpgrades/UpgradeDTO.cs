namespace ThirdPartyDeveloper.Aeria.DemoWeaponUpgrades
{
    [System.Serializable]
    internal sealed class UpgradeDTO
    {
        public int ShardCost;
        public int GoldCost;
        public float Damage;
        public float UserAccuracy;
        public float Accuracy;
        public float UserFireRate;
        public float FireRate;
        public float Reload;
        public float Range;

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(nameof(ShardCost)).Append(": ").AppendLine(ShardCost.ToString());
            sb.Append(nameof(GoldCost)).Append(": ").AppendLine(GoldCost.ToString());
            sb.Append(nameof(Damage)).Append(": ").AppendLine(Damage.ToString());
            sb.Append(nameof(UserAccuracy)).Append(": ").AppendLine(UserAccuracy.ToString());
            sb.Append(nameof(Accuracy)).Append(": ").AppendLine(Accuracy.ToString());
            sb.Append(nameof(UserFireRate)).Append(": ").AppendLine(UserFireRate.ToString());
            sb.Append(nameof(FireRate)).Append(": ").AppendLine(FireRate.ToString());
            sb.Append(nameof(Reload)).Append(": ").AppendLine(Reload.ToString());
            sb.Append(nameof(Range)).Append(": ").AppendLine(Range.ToString());
            return sb.ToString();
        }
    }
}