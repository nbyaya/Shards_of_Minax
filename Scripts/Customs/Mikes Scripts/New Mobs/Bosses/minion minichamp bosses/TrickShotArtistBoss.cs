using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the supreme trick shot artist")]
    public class TrickShotArtistBoss : TrickShotArtist
    {
        [Constructable]
        public TrickShotArtistBoss() : base()
        {
            Name = "Supreme Trick Shot Artist";
            Title = "the Master of Arrows";

            // Update stats to match or exceed Barracoon
            SetStr(900, 1200); // Enhanced strength
            SetDex(300, 400); // Enhanced dexterity
            SetInt(250, 350); // Enhanced intelligence

            SetHits(10000, 12000); // Boss-tier health
            SetDamage(20, 30); // Boss-tier damage

            SetResistance(ResistanceType.Physical, 60, 80); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Anatomy, 100.0, 120.0); // Enhanced skill ranges
            SetSkill(SkillName.Archery, 120.0, 130.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);

            Fame = 22500; // Similar to Barracoon's fame
            Karma = -22500; // Similar to Barracoon's karma

            VirtualArmor = 60; // Boss-tier armor

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
            // Additional boss logic can be added here (e.g., increased difficulty)
        }

        public TrickShotArtistBoss(Serial serial) : base(serial)
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
