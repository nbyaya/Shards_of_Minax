using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Trailpiercer : RangersCrossbow
{
    [Constructable]
    public Trailpiercer()
    {
        Name = "Trailpiercer";
        Hue = Utility.Random(1150, 1250);  // Earthy greenish hues, reflecting nature and precision
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 70);
        Attributes.WeaponSpeed = 5;
        Attributes.Luck = 20;
        
        // Slayer effect – Trailpiercer is especially effective against creatures of the wild, aiding in the defense of forests and lands
        Slayer = SlayerName.ReptilianDeath;
        
        // Weapon attributes for enhanced precision and deadly strikes
        WeaponAttributes.HitLeechHits = 15;
        WeaponAttributes.HitLeechMana = 10;
        WeaponAttributes.HitLeechStam = 10;
        WeaponAttributes.BattleLust = 15;
        
        // Skill bonuses for improving ranged combat and survival tactics in the wild
        SkillBonuses.SetValues(0, SkillName.Tactics, 10.0); // Expertise in battle tactics, especially ranged combat
        SkillBonuses.SetValues(1, SkillName.Hiding, 20.0); // Strengthens the user's archery skills
        SkillBonuses.SetValues(2, SkillName.Tracking, 15.0); // Boosts tracking abilities, perfect for a ranger
        SkillBonuses.SetValues(3, SkillName.AnimalLore, 10.0); // Enhances knowledge of wild creatures

        // Additional thematic bonus
        SkillBonuses.SetValues(4, SkillName.Fletching, 10.0); // Reflecting the ranger's affinity with the bow and arrows

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Trailpiercer(Serial serial) : base(serial)
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
