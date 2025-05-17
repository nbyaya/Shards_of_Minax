using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NaptimeObliterator : SleepAid
{
    [Constructable]
    public NaptimeObliterator()
    {
        Name = "Naptime Obliterator";
        Hue = Utility.Random(1150, 1200);  // A calming but eerie blue, reflecting a weapon's dual purpose
        MinDamage = Utility.RandomMinMax(10, 20);
        MaxDamage = Utility.RandomMinMax(20, 40);
        Attributes.WeaponSpeed = 5;
        Attributes.Luck = 15;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Meditation, 20.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 15.0);
        SkillBonuses.SetValues(2, SkillName.Alchemy, 10.0);

        // Unique effects - sleep-inducing and stamina-draining abilities
        WeaponAttributes.HitLeechStam = 30;  // Drains stamina to tire out the target
        WeaponAttributes.HitHarm = 10;  // Increases lethality with the sleep effect when opponents are too fatigued

        // Slayer Effect - Works especially well against those whose willpower can be broken (e.g. undead, demons)
        Slayer = SlayerName.Exorcism;

        // Attach the XML item for future expansion/interaction in the game world
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NaptimeObliterator(Serial serial) : base(serial)
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
