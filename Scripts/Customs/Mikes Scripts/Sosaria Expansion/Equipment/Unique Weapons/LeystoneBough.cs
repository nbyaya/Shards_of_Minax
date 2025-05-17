using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LeystoneBough : MysticStaff
{
    [Constructable]
    public LeystoneBough()
    {
        Name = "Leystone Bough";
        Hue = Utility.Random(1150, 1260);  // A radiant greenish-blue hue, representing the magical connection to the elements
        MinDamage = Utility.RandomMinMax(15, 30);
        MaxDamage = Utility.RandomMinMax(35, 60);
        Attributes.WeaponSpeed = 10; // To symbolize the staff’s fluidity and connection to magic
        Attributes.Luck = 10; // A small boost to luck, as the Leystone Bough is associated with favorable magical encounters
        
        // Skill Bonuses for magical mastery
        SkillBonuses.SetValues(0, SkillName.Mysticism, 20.0);  // Represents the staff’s bond with elemental and arcane forces
        SkillBonuses.SetValues(1, SkillName.Magery, 15.0);  // Enhances the wielder’s magical power, especially elemental magics
        SkillBonuses.SetValues(2, SkillName.Spellweaving, 10.0);  // The staff aids in weaving arcane energies together
        
        // Slayer effect – Leystone Bough’s magic is specifically effective against elemental creatures
        Slayer = SlayerName.ElementalBan; // Elementals fear the magic of this staff, making it ideal for combating them

        // Weapon Attributes for magic-related abilities
        WeaponAttributes.HitLeechMana = 30;  // Allows the wielder to regain mana with each successful hit, feeding from the staff’s power
        WeaponAttributes.HitColdArea = 20;  // A cold aura emanates from the staff, damaging and slowing enemies in its vicinity
        WeaponAttributes.BattleLust = 15; // The wielder of Leystone Bough gains a sense of purpose, bolstering their drive in combat
        
        XmlAttach.AttachTo(this, new XmlLevelItem());  // Attaches an XML level item for special features or future expansions
    }

    public LeystoneBough(Serial serial) : base(serial)
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
