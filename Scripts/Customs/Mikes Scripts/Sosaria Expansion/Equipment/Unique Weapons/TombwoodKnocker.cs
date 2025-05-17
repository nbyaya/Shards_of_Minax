using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TombwoodKnocker : Club
{
    [Constructable]
    public TombwoodKnocker()
    {
        Name = "Tombwood Knocker";
        Hue = Utility.Random(2300, 2500);  // A deep, haunted green, evoking the ancient woods
        MinDamage = Utility.RandomMinMax(15, 35);
        MaxDamage = Utility.RandomMinMax(40, 60);
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 20;
        
        // Slayer effect – The weapon is especially effective against the undead, perfect for its graveyard origins
        Slayer = SlayerName.SpidersDeath;

        // Weapon attributes - enhancing the user’s offensive and defensive capability
        WeaponAttributes.HitLeechHits = 25;
        WeaponAttributes.HitLeechStam = 20;
        WeaponAttributes.BattleLust = 15;

        // Skill bonuses to enhance combat in graveyard-like environments, dealing with undead and nature’s spirits
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
        SkillBonuses.SetValues(1, SkillName.Macing, 20.0);
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0);

        // Additional thematic bonus – strengthening the bond with nature and spirits
        SkillBonuses.SetValues(3, SkillName.SpiritSpeak, 15.0);

        // Attach XML level item behavior for the unique item’s properties
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TombwoodKnocker(Serial serial) : base(serial)
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
