using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the Supreme Knight of Justice")]
    public class KnightOfJusticeBoss : KnightOfJustice
    {
        [Constructable]
        public KnightOfJusticeBoss() : base()
        {
            Name = "Supreme Knight of Justice";
            Title = "the Unyielding";

            // Update stats to match or exceed Barracoon
            SetStr(1300); // Matching Barracoon's upper strength
            SetDex(200); // Matching Barracoon's upper dexterity
            SetInt(150); // Keeping original intelligence

            SetHits(12000); // Matching Barracoon's health
            SetStam(300); // Matching Barracoon's upper stamina
            SetMana(750); // Matching Barracoon's upper mana

            SetDamage(30, 40); // Increase damage for a boss-tier

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 65, 75);
            SetResistance(ResistanceType.Cold, 65, 75);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 120.0); // Increase skill levels
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Swords, 120.0);
            SetSkill(SkillName.Parry, 110.0);
            SetSkill(SkillName.Chivalry, 100.0);

            Fame = 22500; // Increased fame
            Karma = 22500; // Increased karma (for a positive boss)

            VirtualArmor = 80; // Increase armor

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here
        }

        public KnightOfJusticeBoss(Serial serial) : base(serial)
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
}
