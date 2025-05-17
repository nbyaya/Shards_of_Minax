using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MercysEdge : Scalpel
{
    [Constructable]
    public MercysEdge()
    {
        Name = "Mercy's Edge";
        Hue = Utility.Random(2000, 2100); // Pale, ethereal glow, symbolizing healing and mercy
        MinDamage = Utility.RandomMinMax(5, 15); // Light damage, suited for precise strikes
        MaxDamage = Utility.RandomMinMax(20, 35); // Still formidable in skilled hands
        Attributes.WeaponSpeed = 10; // Increased speed for surgical precision

        // Special Slayer - this scalpel is effective against cursed and undead beings, symbolic of the blade's healing nature
        Slayer = SlayerName.Exorcism;
        
        // Weapon Attributes - focusing on support and healing-based bonuses
        WeaponAttributes.HitLeechHits = 20; // Leeching health to sustain the wielder
        WeaponAttributes.HitLeechMana = 10; // Mana leech to allow for continued use of magic
        WeaponAttributes.HitLeechStam = 5; // Provides stamina for sustained action in battle

        // Skill bonuses - enhancing healing, anatomy, and tactical skills
        SkillBonuses.SetValues(0, SkillName.Healing, 15.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 15.0);
        SkillBonuses.SetValues(2, SkillName.Tactics, 10.0);
        
        // Additional thematic bonus to emphasize surgical precision and restorative powers
        SkillBonuses.SetValues(3, SkillName.Veterinary, 10.0);
        
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MercysEdge(Serial serial) : base(serial)
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
