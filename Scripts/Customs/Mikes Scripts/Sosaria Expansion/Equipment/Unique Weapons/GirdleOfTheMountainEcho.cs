using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GirdleOfTheMountainEcho : WrestlingBelt
{
    [Constructable]
    public GirdleOfTheMountainEcho()
    {
        Name = "Girdle of the Mountain Echo";
        Hue = Utility.Random(2100, 2300);  // A deep earthy hue, symbolizing mountain stone and strength
        Weight = 5.0;
        
        Attributes.BonusStr = 15;  // Represents the physical power granted by the belt
        Attributes.BonusDex = 10;  // Enhances agility, aiding in swift wrestling movements
        
        // Increased stamina and hit points for resilience in combat
        Attributes.BonusHits = 30;
        Attributes.BonusStam = 25;
        
        // Defend and Attack boosts, reflecting the skill of a wrestler
        Attributes.DefendChance = 10;
        Attributes.AttackChance = 10;

        // Adds a thematic bonus, improving wrestling ability and physical prowess
        SkillBonuses.SetValues(0, SkillName.Wrestling, 20.0);  // Strong wrestling bonus to reinforce combat
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);  // Adds strategic advantage to moves
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0);  // Reflects the power of strikes and grapples
        
        // Adds a lore-appropriate Slayer effect: The "Mountain Echo" implies a connection to earth elementals or titans
        Slayer = SlayerName.ElementalBan;

        // Additional effects reflecting the earth connection
        WeaponAttributes.HitLeechHits = 10;
        WeaponAttributes.HitLeechStam = 5;

        // Attach the item to the XmlLevelItem for progression
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GirdleOfTheMountainEcho(Serial serial) : base(serial)
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
