using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the spellbreaker overlord")]
    public class SpellbreakerBoss : Spellbreaker
    {
        [Constructable]
        public SpellbreakerBoss() : base()
        {
            Name = "Spellbreaker Overlord";
            Title = "the Supreme Dispelter";

            // Update stats to match or exceed Barracoon's stats
            SetStr(700); // Upper strength value
            SetDex(150); // Upper dexterity value
            SetInt(600); // Upper intelligence value

            SetHits(12000); // Increased health
            SetDamage(29, 38); // Increased damage range

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 120.0); // Boosted MagicResist skill
            SetSkill(SkillName.Tactics, 100.0); // Boosted Tactics skill
            SetSkill(SkillName.Wrestling, 100.0); // Boosted Wrestling skill

            Fame = 22500; // Boosted Fame
            Karma = -22500; // Boosted Karma

            VirtualArmor = 70; // Boosted armor

            // Attach the XmlRandomAbility
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
            // Additional boss logic could be added here (e.g., special moves, aura, etc.)
        }

        public SpellbreakerBoss(Serial serial) : base(serial)
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
