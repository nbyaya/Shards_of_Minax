using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThistlesThorn : Bardiche
{
    [Constructable]
    public ThistlesThorn()
    {
        Name = "Thistle’s Thorn";
        Hue = Utility.Random(1150, 1200);  // A shade of deep green, representing the thorns of nature
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 75);
        Attributes.WeaponSpeed = 5;  // Slightly faster to match the quick nature of thorns
        Attributes.Luck = 15;  // A touch of fortune in the wilds

        // Slayer effect – Thistle's Thorn is particularly deadly to those who seek to harm nature
        Slayer = SlayerName.ReptilianDeath;  // Reptiles, as they often lurk in forests or fields, enemies of nature

        // Weapon attributes - providing a natural synergy in defense and offense with nature's power
        WeaponAttributes.HitLeechHits = 20;  // The thorns seem to leech life from enemies
        WeaponAttributes.HitLeechStam = 10;  // The sting of the thorns saps stamina
        WeaponAttributes.HitEnergyArea = 25;  // Nature can strike in bursts of energy, reflecting thorns’ unpredictable danger

        // Skill bonuses tied to nature and defense
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);  // Enhancing tactical combat
        SkillBonuses.SetValues(1, SkillName.Swords, 20.0);  // Focused on the physical mastery of the blade
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0);  // An understanding of the body's weak points, like thorns piercing

        // Thematic bonus - better performance against reptilian creatures, which harm nature
        SkillBonuses.SetValues(3, SkillName.Healing, 5.0);  // Bonus for self-healing, the thorns encourage quick recovery after battle

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThistlesThorn(Serial serial) : base(serial)
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
