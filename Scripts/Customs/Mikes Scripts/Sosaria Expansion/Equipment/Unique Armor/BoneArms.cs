using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RemnantsOfTheGraveMarch : BoneArms
{
    [Constructable]
    public RemnantsOfTheGraveMarch()
    {
        Name = "Remnants of the Grave March";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);

        ArmorAttributes.SelfRepair = 5;
        Attributes.DefendChance = 15;
        Attributes.LowerManaCost = 10;
        Attributes.SpellDamage = 12;
        Attributes.NightSight = 1;

        // Necromancy and related skills
        SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 10.0);
        SkillBonuses.SetValues(2, SkillName.Tactics, 15.0);

        // Elemental resistances fit for undead-themed armor
        ColdBonus = 10;
        PoisonBonus = 15;

        // XML level item attachment
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RemnantsOfTheGraveMarch(Serial serial) : base(serial)
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
