using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Forgeheart : SmithSmasher
{
    [Constructable]
    public Forgeheart()
    {
        Name = "Forgeheart";
        Hue = Utility.Random(1150, 1290);  // A fiery red hue to represent the forge's heat
        MinDamage = Utility.RandomMinMax(35, 55);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.WeaponSpeed = 5;
        Attributes.Luck = 15;

        // This weapon is especially effective against metal-based creatures, like Golems or other constructs
        Slayer = SlayerName.ElementalBan;

        // Weapon attributes to tie it to its forging nature
        WeaponAttributes.HitLeechHits = 20;
        WeaponAttributes.HitLeechMana = 10;
        WeaponAttributes.HitLeechStam = 15;
        WeaponAttributes.BattleLust = 15;
        
        // Skill bonuses to emphasize its creation and smithing roots
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 15.0);  // Tied to its origin in the forge
        SkillBonuses.SetValues(1, SkillName.Tactics, 20.0);      // Increased effectiveness in battle
        SkillBonuses.SetValues(2, SkillName.Mining, 10.0);       // Tied to its origin in the mines and smithing
        
        // Additional thematic bonus reflecting the power of forge-crafted weapons
        SkillBonuses.SetValues(3, SkillName.MagicResist, 5.0);    // A resistance gained from the forge’s heated, magical nature

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Forgeheart(Serial serial) : base(serial)
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
