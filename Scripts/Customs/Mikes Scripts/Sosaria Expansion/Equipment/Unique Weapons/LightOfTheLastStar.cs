using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LightOfTheLastStar : CampingLanturn
{
    [Constructable]
    public LightOfTheLastStar()
    {
        Name = "Light of the Last Star";
        Hue = Utility.Random(1250, 1350);  // A soft, ethereal glow of pale blue light, representing the fading light of a dying star.
        Attributes.Luck = 30;
        Attributes.RegenMana = 3;
        Attributes.RegenStam = 3;
        MinDamage = Utility.RandomMinMax(20, 45);
        MaxDamage = Utility.RandomMinMax(50, 80);		

        // Special thematic ability - provides a radiant light in the darkest of nights, revealing hidden dangers and secrets.
        Attributes.NightSight = 1;  // Grants night sight, allowing the wielder to see in darkness as if it were daylight.

        // Skill Bonuses: Increases survival and exploration-related skills.
        SkillBonuses.SetValues(0, SkillName.Camping, 20.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 15.0);
        SkillBonuses.SetValues(2, SkillName.Meditation, 10.0);

        // Slayer effect: The lantern illuminates the path to hidden treasures and will scare away certain dark creatures.
        Slayer = SlayerName.SpidersDeath;  // This lantern repels dark, arachnid-like entities, perfect for exploring caves or dark forests.

        // Additional beneficial bonuses for explorers or those who venture into the wilderness
        WeaponAttributes.HitDispel = 50;  // Helps dispel harmful magical effects while adventuring.
        WeaponAttributes.HitLeechHits = 20;  // Leech some health when struck, ideal for surviving long, harsh expeditions.

        // Thematic and practical lore: This lantern is a beacon of hope for those traveling in dark places, protecting against the darkness itself.
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LightOfTheLastStar(Serial serial) : base(serial)
    {
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}
