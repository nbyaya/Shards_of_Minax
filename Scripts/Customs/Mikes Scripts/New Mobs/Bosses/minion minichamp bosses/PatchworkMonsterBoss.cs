using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a patchwork overlord corpse")]
    public class PatchworkMonsterBoss : PatchworkMonster
    {
        [Constructable]
        public PatchworkMonsterBoss() : base()
        {
            Name = "Patchwork Overlord";
            Title = "the Unstoppable";

            // Enhanced stats for the boss version
            SetStr(1400); // Max strength for more durability
            SetDex(300); // Max dexterity for agility
            SetInt(200); // Increased intelligence for better skill performance

            SetHits(12000); // High health like Barracoon

            SetDamage(29, 38); // Similar damage range as Barracoon

            SetResistance(ResistanceType.Physical, 90, 100); // Max physical resistance
            SetResistance(ResistanceType.Fire, 70, 90); // Fire resistance
            SetResistance(ResistanceType.Cold, 90, 100); // Cold resistance
            SetResistance(ResistanceType.Poison, 100, 100); // Max poison resistance
            SetResistance(ResistanceType.Energy, 70, 90); // Energy resistance

            SetSkill(SkillName.MagicResist, 120.0); // Strong magic resistance
            SetSkill(SkillName.Tactics, 100.0); // Strong tactics skill
            SetSkill(SkillName.Wrestling, 100.0); // Powerful wrestling skill

            Fame = 22500; // High fame to indicate boss difficulty
            Karma = -22500; // Negative karma for a villainous boss

            VirtualArmor = 70; // High virtual armor

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
            // Additional boss-specific behavior could be added here, if desired
        }

        public PatchworkMonsterBoss(Serial serial) : base(serial)
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
