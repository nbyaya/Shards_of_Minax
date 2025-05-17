using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TouchOfTheVoid : TenFootPole
{
    [Constructable]
    public TouchOfTheVoid()
    {
        Name = "Touch of the Void";
        Hue = Utility.Random(2400, 2500); // A shadowy, dark hue representing the void, with swirling purple and black.
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 80);
        Attributes.WeaponSpeed = 5;
        Attributes.Luck = 20;
        
        // Slayer Effect – Touch of the Void is effective against supernatural and magical creatures
        Slayer = SlayerName.Vacuum; // Represents the idea of sucking life and energy into nothingness.

        // Weapon Attributes - Embodying the void and its eerie power
        WeaponAttributes.HitLeechHits = 15;
        WeaponAttributes.HitLeechMana = 25;
        WeaponAttributes.HitLowerDefend = 10;

        // Skill Bonuses that align with the item’s connection to the void and arcane powers
        SkillBonuses.SetValues(0, SkillName.Mysticism, 20.0); // Mysticism for its connection to arcane powers
        SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0); // EvalInt enhances its power in terms of understanding arcane energies
        SkillBonuses.SetValues(2, SkillName.Spellweaving, 10.0); // Spellweaving adds a mystical weaving of reality
        SkillBonuses.SetValues(3, SkillName.Necromancy, 5.0); // Necromancy for its dark, otherworldly influence

        // Thematic Bonus - Leeching the very essence of life, weakening enemies.
        Attributes.CastSpeed = 1; // Adds an arcane element of magic casting speed, reinforcing the void theme

        // Attach the item as a unique weapon
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TouchOfTheVoid(Serial serial) : base(serial)
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
