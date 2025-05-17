using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShattermarchGreaves : PlateSuneate
{
    [Constructable]
    public ShattermarchGreaves()
    {
        Name = "Shattermarch Greaves";
        Hue = Utility.Random(2200, 2300); // Dark, stormy hue to evoke the "shattering" theme
        BaseArmorRating = Utility.RandomMinMax(30, 70); // Sturdy plate armor for reliable protection

        ArmorAttributes.SelfRepair = 5; // Armor can repair itself after battles
        Attributes.DefendChance = 10; // Increases the chance to defend against attacks
        Attributes.ReflectPhysical = 5; // Reflects a small portion of physical damage back to attackers
        Attributes.BonusStr = 5; // Adds strength to the wearer, making them more robust in combat
        Attributes.BonusDex = 3; // Adds dexterity, helping to avoid some attacks and improve movement
        Attributes.BonusStam = 10; // Boosts stamina for longer engagements and sprinting
        Attributes.CastSpeed = 1; // Slight increase in casting speed, aiding spellcasting in combat

        SkillBonuses.SetValues(0, SkillName.Tactics, 10.0); // Provides an advantage in tactical combat
        SkillBonuses.SetValues(1, SkillName.Swords, 10.0); // Boosts swordsmanship, ensuring effectiveness in melee
        SkillBonuses.SetValues(2, SkillName.Parry, 10.0); // Improves the ability to parry incoming blows

        ColdBonus = 5; // Provides resistance to cold-based attacks, useful in harsh environments
        FireBonus = 5; // Also offers resistance to fire, giving a balanced approach to elemental damage

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach custom functionality from XML level system
    }

    public ShattermarchGreaves(Serial serial) : base(serial)
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
