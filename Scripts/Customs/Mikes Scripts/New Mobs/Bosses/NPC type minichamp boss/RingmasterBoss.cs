using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the grand ringmaster")]
    public class RingmasterBoss : Ringmaster
    {
        [Constructable]
        public RingmasterBoss() : base()
        {
            Name = "Grand Ringmaster";
            Title = "the Boss of the Circus";

            // Update stats to match or exceed Barracoon's as baseline
            SetStr(1200); // Stronger than original Ringmaster
            SetDex(255);  // Upper dexterity value
            SetInt(250);  // Higher intelligence

            SetHits(12000); // Boss-tier health
            SetDamage(30, 50); // Increased damage range for a more dangerous boss

            SetResistance(ResistanceType.Physical, 75, 85); // Stronger resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 65, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Max skill
            SetSkill(SkillName.Tactics, 120.0);     // Stronger tactics
            SetSkill(SkillName.Wrestling, 120.0);   // Stronger wrestling
            SetSkill(SkillName.Magery, 100.0);      // High magery skill
            SetSkill(SkillName.EvalInt, 100.0);     // High evalint skill

            Fame = 22500; // Increased fame for the boss
            Karma = -22500; // Negative karma for the boss

            VirtualArmor = 80; // Stronger virtual armor

            // Attach random ability for dynamic combat
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here if needed
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Add extra rich loot for a boss encounter
            PackGem();
            PackGold(500, 1000);
            AddLoot(LootPack.UltraRich);  // Even richer loot than normal
        }

        public RingmasterBoss(Serial serial) : base(serial)
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
