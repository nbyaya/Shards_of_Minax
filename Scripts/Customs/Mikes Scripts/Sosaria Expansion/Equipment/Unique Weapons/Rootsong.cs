using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Rootsong : GnarledStaff
{
    [Constructable]
    public Rootsong()
    {
        Name = "Rootsong";
        Hue = Utility.Random(0, 2500);  // A natural earthy tone, blending shades of green and brown
        MinDamage = Utility.RandomMinMax(15, 25);
        MaxDamage = Utility.RandomMinMax(30, 50);
        
        // Attributes themed around nature and healing
        Attributes.BonusInt = 10;  // Amplifying intelligence for spellcasters
        Attributes.BonusMana = 15;  // Increased mana to strengthen spellcasting
        Attributes.CastSpeed = 1;  // Faster casting time

        // Skill bonuses – promoting nature-based and magical skills
        SkillBonuses.SetValues(0, SkillName.Mysticism, 15.0);
        SkillBonuses.SetValues(1, SkillName.Spellweaving, 10.0);
        SkillBonuses.SetValues(2, SkillName.Healing, 10.0);

        // Slayer effect – it is especially effective against creatures connected to the undead, representing the life force of nature overcoming death
        Slayer = SlayerName.ArachnidDoom;

        // Weapon Attributes – giving the staff additional magical effects
        WeaponAttributes.HitLeechMana = 20;
        WeaponAttributes.HitLeechHits = 10;

        // Additional thematic bonus for nature-based healing and protection
        WeaponAttributes.HitDispel = 15;

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Rootsong(Serial serial) : base(serial)
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
