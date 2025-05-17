using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TongueOfTheGods : GourmandsFork
{
    [Constructable]
    public TongueOfTheGods()
    {
        Name = "Tongue of the Gods";
        Hue = Utility.Random(2100, 2200); // A rich, divine golden hue, symbolizing the connection to the gods
        MinDamage = Utility.RandomMinMax(15, 30);
        MaxDamage = Utility.RandomMinMax(35, 55); 
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 50;

        // Slayer effect – Tongue of the Gods is particularly effective when used in culinary rituals or sacred feasts
        Slayer = SlayerName.EodonTribe;

        // Weapon attributes related to consuming or cooking
        WeaponAttributes.BattleLust = 30;
        WeaponAttributes.HitLeechMana = 10;
        WeaponAttributes.HitLeechHits = 10;
        
        // Skill bonuses to enhance the player’s cooking expertise and alchemical abilities
        SkillBonuses.SetValues(0, SkillName.Cooking, 20.0);
        SkillBonuses.SetValues(1, SkillName.Alchemy, 15.0);
        SkillBonuses.SetValues(2, SkillName.TasteID, 20.0);

        // Additional thematic bonus for enhancing physical consumption and preparing divine meals
        SkillBonuses.SetValues(3, SkillName.Anatomy, 10.0);

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TongueOfTheGods(Serial serial) : base(serial)
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
