using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a puffy ferret corpse")]
    public class PuffyFerretBoss : PuffyFerret
    {
        [Constructable]
        public PuffyFerretBoss() : base()
        {
            // Enhance the stats to match or exceed typical boss level
            Name = "Puffy Ferret Overlord";
            Title = "the Fluffy Tyrant";

            SetStr(1200); // Upper bound for strength
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence

            SetHits(12000); // Boss level health
            SetDamage(35, 50); // Increased damage range for difficulty

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 110.0);
            SetSkill(SkillName.Wrestling, 110.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.Meditation, 50.0);

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma penalty

            VirtualArmor = 120; // Enhanced virtual armor

            PackItem(new BossTreasureBox());
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

            // Add extra rich loot as befits a boss-level creature
            this.AddLoot(LootPack.FilthyRich, 2);
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Gems, 8);
        }

        public override void OnThink()
        {
            base.OnThink();
            // Boss-specific behaviors or timing tweaks can be added here if necessary
        }

        public PuffyFerretBoss(Serial serial)
            : base(serial)
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
