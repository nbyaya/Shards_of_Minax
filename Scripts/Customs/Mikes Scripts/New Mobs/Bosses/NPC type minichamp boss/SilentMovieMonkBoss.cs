using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the silent movie overlord")]
    public class SilentMovieMonkBoss : SilentMovieMonk
    {
        [Constructable]
        public SilentMovieMonkBoss() : base()
        {
            Name = "Silent Movie Overlord";
            Title = "the Supreme Silent";

            // Update stats to match or exceed Barracoon's as a boss
            SetStr(1200); // Matching the high strength
            SetDex(255); // Max dexterity for a more agile boss
            SetInt(250); // High intelligence to match boss tier

            SetHits(12000); // Boss-tier health
            SetDamage(20, 35); // Increased damage range

            SetResistance(ResistanceType.Physical, 75); // Maxed out physical resistance
            SetResistance(ResistanceType.Fire, 80); // Maxed out fire resistance
            SetResistance(ResistanceType.Cold, 70); // Strong cold resistance
            SetResistance(ResistanceType.Poison, 100); // Max poison resistance
            SetResistance(ResistanceType.Energy, 60); // Strong energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // Increased Magic Resistance
            SetSkill(SkillName.Tactics, 120.0); // Boss-level tactics
            SetSkill(SkillName.Wrestling, 120.0); // Higher wrestling skill

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70; // Higher armor to withstand more damage

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
            // Additional boss logic can be implemented here
        }

        public SilentMovieMonkBoss(Serial serial) : base(serial)
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
