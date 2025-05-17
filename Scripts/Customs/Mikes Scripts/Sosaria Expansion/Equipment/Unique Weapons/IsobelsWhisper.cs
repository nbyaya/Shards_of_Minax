using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class IsobelsWhisper : MageWand
{
    [Constructable]
    public IsobelsWhisper()
    {
        Name = "Isobel's Whisper";
        Hue = Utility.Random(1500, 1800);  // A muted lavender hue, representing mysticism and secrecy
        MinDamage = Utility.RandomMinMax(10, 20);
        MaxDamage = Utility.RandomMinMax(30, 50);
        
        Attributes.CastSpeed = 1;  // Increased cast speed for faster spells
        Attributes.CastRecovery = 1;  // Faster recovery between spells
        Attributes.LowerManaCost = 15;  // Reduced mana cost for casting
        Attributes.SpellDamage = 15;  // Increased spell damage output
        
        // Slayer effect – Isobel's Whisper is potent against magic-wielding foes, a perfect tool for the curious mage
        Slayer = SlayerName.ElementalBan;
        
        // Weapon attributes - enhancing the mage's abilities in magic and evasion
        WeaponAttributes.HitDispel = 50;  // Ability to dispel magic with a successful hit
        WeaponAttributes.HitLeechMana = 25;  // Leech mana from enemies to fuel your own spellcasting
        WeaponAttributes.HitEnergyArea = 40;  // Deals energy damage in an area effect
        
        // Skill bonuses enhancing magical and analytical prowess
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);  // Increases Magery skill for spellcasting power
        SkillBonuses.SetValues(1, SkillName.EvalInt, 10.0);  // Boosts Evaluating Intelligence to better analyze magical resistances
        SkillBonuses.SetValues(2, SkillName.Meditation, 10.0);  // Helps with mana regeneration, vital for a mage using the wand
        
        // Thematic bonus: magical precision and the ability to communicate with spirits
        SkillBonuses.SetValues(3, SkillName.SpiritSpeak, 5.0);  // A subtle touch of the spirit world to aid in communication
        
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public IsobelsWhisper(Serial serial) : base(serial)
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
