using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Quillfang : ScribeSword
{
    [Constructable]
    public Quillfang()
    {
        Name = "Quillfang";
        Hue = Utility.Random(1150, 1200);  // A pale shade of silver and ink to represent the connection between knowledge and power.
        MinDamage = Utility.RandomMinMax(15, 30);
        MaxDamage = Utility.RandomMinMax(35, 60);
        Attributes.WeaponSpeed = 5;  // This weapon strikes with precision and swiftness, like a well-written passage.
        Attributes.Luck = 15;  // A small amount of fortune, for those who walk the path of knowledge.
        
        // Slayer Effect: Quillfang is designed to tear through creatures of the mind or spirits.
        Slayer = SlayerName.ArachnidDoom;  // A nod to the complex, intricate webs of knowledge and the dangers lurking within.

        // Weapon Attributes - Adding a little magical potency in the form of Leech effects, representative of the siphoning of knowledge.
        WeaponAttributes.HitLeechHits = 20;  // Leech some of the damage back to the user.
        WeaponAttributes.HitLeechMana = 15;  // Absorb mana to fuel the wielder's wisdom.
        WeaponAttributes.HitLeechStam = 10;  // Steal stamina from opponents in the fight.

        // Skill Bonuses - Focus on knowledge-related skills as well as enhancing the sword's function in battle.
        SkillBonuses.SetValues(0, SkillName.Inscribe, 15.0);  // The weapon enhances your skill with written lore and inscriptions.
        SkillBonuses.SetValues(1, SkillName.Mysticism, 10.0);  // Gives an arcane edge to the wielder, allowing them to tap into mystic energies.
        SkillBonuses.SetValues(2, SkillName.EvalInt, 10.0);  // Sharpens the intellect, making you more aware of hidden truths in battle and study.

        // Additional thematic bonus – A tie to ancient forces of magic or knowledge.
        Attributes.SpellDamage = 5;  // A slight increase to spell damage, in line with the intellectual nature of the weapon.

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Quillfang(Serial serial) : base(serial)
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
