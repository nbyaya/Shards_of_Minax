using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Lifebringer : VeterinaryLance
{
    [Constructable]
    public Lifebringer()
    {
        Name = "Lifebringer";
        Hue = Utility.Random(1150, 1190); // A soft, healing greenish hue symbolizing life and nature
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 75);
        Attributes.WeaponSpeed = 5; // Slightly faster to allow quick strikes
        Attributes.Luck = 15; // Luck for finding animals to heal or help
        


        // Weapon attributes - Healing abilities to aid companions and creatures in battle
        WeaponAttributes.HitLeechHits = 20; // Allows the wielder to heal themselves with each strike
        WeaponAttributes.HitLeechStam = 15; // Provides stamina regeneration with each strike
        WeaponAttributes.HitLeechMana = 10; // A minor amount of mana leech to aid in healing spells

        // Skill bonuses that match the lance's connection to animal care and healing
        SkillBonuses.SetValues(0, SkillName.Veterinary, 20.0); // Increases effectiveness in animal care
        SkillBonuses.SetValues(1, SkillName.Healing, 15.0); // Enhances the healing ability of the wielder
        SkillBonuses.SetValues(2, SkillName.Tactics, 10.0); // Provides tactical advantage for taking care of the team
        
        // Additional thematic bonus for enhancing defensive abilities while protecting allies
        SkillBonuses.SetValues(3, SkillName.Anatomy, 10.0); // Allows for a deeper understanding of wounds and injuries

        // XmlLevelItem attachment for custom functionality
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Lifebringer(Serial serial) : base(serial)
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
