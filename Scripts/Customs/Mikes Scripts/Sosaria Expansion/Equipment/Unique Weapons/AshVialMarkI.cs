using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AshVialMarkI : FireAlchemyBlaster
{
    [Constructable]
    public AshVialMarkI()
    {
        Name = "Ash Vial Mark I";
        Hue = 1355;  // A fiery orange-red, representing its alchemical power
        ItemID = 0x13F6;  // A placeholder graphic for a ranged alchemical weapon
        
        // The AshVialMarkI specializes in fire-based attacks and alchemical skill bonuses
        Attributes.BonusStr = 5;
        Attributes.BonusInt = 15;
        Attributes.BonusMana = 20;
        Attributes.Luck = 15;
        Attributes.WeaponSpeed = 10;
        
        // Hit effects – the weapon specializes in fire damage and damage over time
        WeaponAttributes.HitFireball = 25;
        WeaponAttributes.HitFireArea = 20;
        WeaponAttributes.HitHarm = 10;
        
        // A thematic alchemy bonus that enhances the wielder's magical and alchemical skills
        SkillBonuses.SetValues(0, SkillName.Alchemy, 25.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);
        SkillBonuses.SetValues(2, SkillName.MagicResist, 10.0);

        // Slayer effect – this weapon is particularly effective against certain fire-based enemies
        Slayer = SlayerName.FlameDousing;

        // Attach XML Level Item for advanced interaction
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AshVialMarkI(Serial serial) : base(serial)
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
