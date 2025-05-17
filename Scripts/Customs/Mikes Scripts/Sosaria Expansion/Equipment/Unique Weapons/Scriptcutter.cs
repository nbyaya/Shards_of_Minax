using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Scriptcutter : LoreSword
{
    [Constructable]
    public Scriptcutter()
    {
        Name = "Scriptcutter";
        Hue = Utility.Random(1350, 1400);  // A mystical shade representing knowledge and magic
        MinDamage = Utility.RandomMinMax(40, 60);
        MaxDamage = Utility.RandomMinMax(70, 100);

        // Adding attributes related to both intellect and combat
        Attributes.WeaponSpeed = 10; // Enhances combat speed, representing the swiftness with which secrets are uncovered
        Attributes.BonusInt = 15; // The sword enhances the wielder’s intellect, aiding in uncovering mysteries
        Attributes.BonusMana = 20; // Increases mana, representing the connection to arcane power

        // Skill bonuses related to lore, magic, and combat tactics
        SkillBonuses.SetValues(0, SkillName.Inscribe, 15.0);  // Tied to ancient scripts and runes
        SkillBonuses.SetValues(1, SkillName.EvalInt, 10.0);    // Increases evaluation of magical knowledge
        SkillBonuses.SetValues(2, SkillName.Chivalry, 20.0);     // Enhances swordsmanship for skilled combatants

        // Slayer effect – the sword is especially effective against those who manipulate arcane knowledge
        Slayer = SlayerName.ArachnidDoom;  // Given that many secrets are often guarded by ancient, web-like conspiracies

        // Weapon attributes that enhance the sword’s utility and versatility
        WeaponAttributes.HitLeechHits = 15;    // Leech health from enemies, representing the draining of knowledge or life force
        WeaponAttributes.HitLeechMana = 15;    // Leech mana, as if cutting through the barriers of the mind

        // Adding some thematic bonuses for those dedicated to unraveling secrets and magic
        Attributes.Luck = 10; // Increases luck, helping the wielder find rare texts and uncover hidden lore
        Attributes.CastSpeed = 1;  // Improves casting speed, aiding those who use magic alongside swordplay

        // Attach the custom level item
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Scriptcutter(Serial serial) : base(serial)
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
