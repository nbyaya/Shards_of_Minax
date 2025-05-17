using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Masksplitter : RevealingAxe
{
    [Constructable]
    public Masksplitter()
    {
        Name = "Masksplitter";
        Hue = Utility.Random(1250, 1290);  // A shade of shadowy, ethereal gray, signifying its ability to unveil hidden truths
        MinDamage = Utility.RandomMinMax(40, 60);
        MaxDamage = Utility.RandomMinMax(80, 120);
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 15;
        
        // Slayer effect - Masksplitter is especially potent against those who hide their true selves or intentions
        Slayer = SlayerName.SpidersDeath;  // Masksplitter's true power is revealed when used against deceitful or hidden enemies
        
        // Weapon attributes
        WeaponAttributes.HitLeechHits = 30;
        WeaponAttributes.HitLeechMana = 20;
        WeaponAttributes.HitLeechStam = 15;
        WeaponAttributes.BattleLust = 25;
        WeaponAttributes.HitDispel = 20;  // Reveals hidden enemies or magic
        
        // Skill bonuses for uncovering hidden truths and combat
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
        SkillBonuses.SetValues(1, SkillName.DetectHidden, 25.0);  // The axe requires expert swordsmanship to properly reveal the unseen
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0);  // A keen eye for spotting weaknesses, as the axe exposes secrets
        
        // Additional thematic bonus - tied to uncovering hidden forces or truths
        SkillBonuses.SetValues(3, SkillName.DetectHidden, 15.0);  // Enhances the wielder's ability to reveal the unseen
        
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Masksplitter(Serial serial) : base(serial)
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
