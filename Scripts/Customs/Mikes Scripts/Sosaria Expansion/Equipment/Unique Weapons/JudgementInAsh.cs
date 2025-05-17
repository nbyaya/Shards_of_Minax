using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class JudgementInAsh : ExecutionersAxe
{
    [Constructable]
    public JudgementInAsh()
    {
        Name = "Judgement in Ash";
        Hue = 1102;  // A dark grayish hue, evoking the ashes of a long-dead world
        MinDamage = Utility.RandomMinMax(40, 60);
        MaxDamage = Utility.RandomMinMax(70, 100);
        
        // Weapon attributes, reflecting its brutal and judgmental nature
        Attributes.WeaponSpeed = 5;
        Attributes.Luck = 15;
        
        // Slayer: This axe is most effective against the undead, those who are 'judged' by time
        Slayer = SlayerName.Exorcism;

        // Special abilities for combat
        WeaponAttributes.HitLeechHits = 30;  // Drains health from enemies struck by it, a nod to its "execution" role
        WeaponAttributes.HitLeechStam = 20;  // Also leeches stamina, weakening foes further
        WeaponAttributes.HitLeechMana = 10;  // And leeches mana, as its strike is both physical and arcane in nature
        
        // Skill bonuses that enhance its effectiveness in execution-style combat
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);  // Boosts tactics for more precise strikes
        SkillBonuses.SetValues(1, SkillName.Lumberjacking, 25.0);  // A significant bonus to swordsmanship, due to its weight and use
        SkillBonuses.SetValues(2, SkillName.Necromancy, 15.0);  // Adds a small bonus to macing skills, as this is a heavy weapon
        SkillBonuses.SetValues(3, SkillName.Discordance, 10.0);  // Reflects its knowledge of anatomy and how to strike to deal mortal wounds


        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public JudgementInAsh(Serial serial) : base(serial)
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
