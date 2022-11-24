namespace ThirdPartyDeveloper.Aeria.DemoWeaponUpgrades
{
    [System.Serializable]
    internal sealed class WeaponDTO
    {
        public string ID;
        public string Rarity;
        public string Type;
        public UpgradeDTO[] Upgrades;

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(nameof(ID)).Append(": ").AppendLine(ID);
            sb.Append(nameof(Rarity)).Append(": ").AppendLine(Rarity);
            sb.Append(nameof(Type)).Append(": ").AppendLine(Type);
            sb.Append(nameof(Upgrades)).Append(" Count: ").AppendLine(Upgrades.Length.ToString());
            for (int i = 0; i < Upgrades.Length; i++)
            {
                sb.Append("[").Append(i).AppendLine("]:");
                sb.Append(Upgrades[i].ToString());
            }
            return sb.ToString();
        }
    }
}