using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Glimmerchant : MagicWand
{
    [Constructable]
    public Glimmerchant()
    {
        Name = "Glimmerchant";
        Hue = Utility.Random(1300, 1400);  // A shimmering, ethereal glow, reminiscent of moonlight
        MinDamage = Utility.RandomMinMax(10, 20);  // Low damage due to its focus on magical enhancement
        MaxDamage = Utility.RandomMinMax(25, 40); 
        Attributes.CastSpeed = 1;  // Faster spellcasting
        Attributes.CastRecovery = 2;  // Faster recovery after casting spells
        Attributes.LowerManaCost = 10;  // Reduces mana cost for spells
        Attributes.SpellDamage = 15;  // Increases the effectiveness of spells
        Attributes.Luck = 15;  // Represents the wand's chance to give good fortune to its wielder
        Attributes.RegenMana = 5;  // Regenerates mana over time, perfect for sustained spellcasting

        // Slayer effect – The Glimmerchant wand is particularly potent against illusions and magic entities
        Slayer = SlayerName.ElementalBan;

        // Weapon attributes – Enhance the wand's magical capabilities
        WeaponAttributes.HitEnergyArea = 25;  // Energy-based area effect attack
        WeaponAttributes.HitFireball = 20;  // Adds Fireball spell-like damage when used in combat
        WeaponAttributes.HitLeechMana = 15;  // Leaches mana from enemies during combat, boosting the wielder's mana pool

        // Skill bonuses for enhancing the user's magical capabilities
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);  // Boosts Magery skill for more effective spellcasting
        SkillBonuses.SetValues(1, SkillName.Meditation, 15.0);  // Improves the ability to meditate and restore mana
        SkillBonuses.SetValues(2, SkillName.EvalInt, 10.0);  // Enhances the user's intelligence for magical efficacy

        // Additional thematic bonus
        SkillBonuses.SetValues(3, SkillName.Mysticism, 10.0);  // Adds a bit of mysticism, perfect for more obscure magic

        XmlAttach.AttachTo(this, new XmlLevelItem());  // This allows the item to have a unique level-based progression if needed
    }

    public Glimmerchant(Serial serial) : base(serial)
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
