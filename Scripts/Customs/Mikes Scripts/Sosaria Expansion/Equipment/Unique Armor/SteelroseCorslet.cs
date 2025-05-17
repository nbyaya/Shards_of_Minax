using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SteelroseCorslet : FemalePlateChest
{
    [Constructable]
    public SteelroseCorslet()
    {
        Name = "Steelrose Corslet";
        Hue = Utility.Random(1000, 2000); // A soft, metallic pinkish hue, evoking the steel and rose imagery.
        BaseArmorRating = Utility.RandomMinMax(40, 80); // A solid armor rating, befitting its unique status.

        ArmorAttributes.SelfRepair = 15; // Represents the durability of the corslet.
        Attributes.BonusStr = 10; // Strength bonus for durability and combat prowess.
        Attributes.BonusDex = 10; // Dexterity bonus, enhancing agility and evasion in combat.
        Attributes.DefendChance = 10; // Slightly improves defense against attacks.

        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0); // Enhances the wearer’s combat tactics.
        SkillBonuses.SetValues(1, SkillName.Swords, 10.0); // A bonus to swordsmanship, implying this corslet is tailored for melee combat.
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0); // The corslet’s design suggests a connection to understanding the body and applying it in combat.

        ColdBonus = 5; // Provides some resistance to cold-based attacks, reflecting the corslet's mystical origins.
        FireBonus = 5; // A slight fire resistance to protect against fiery magic or physical attacks.

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SteelroseCorslet(Serial serial) : base(serial)
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
