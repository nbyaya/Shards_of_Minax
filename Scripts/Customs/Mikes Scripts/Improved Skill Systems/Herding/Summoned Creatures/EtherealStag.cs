using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an ethereal stag corpse")]
    public class EtherealStag : BaseCreature
    {
        private DateTime m_NextMysticAura;

        [Constructable]
        public EtherealStag()
            : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "an ethereal stag";
            Body = 0xED;
            BaseSoundID = 0x82;
            Hue = 1153; // Light blue hue

            this.SetStr(180);
            this.SetDex(130);
            this.SetInt(200);

            this.SetHits(120, 150);
            this.SetMana(0);

            this.SetDamage(12, 18);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 40, 50);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 15, 25);
            this.SetResistance(ResistanceType.Energy, 50, 60);

            this.SetSkill(SkillName.MagicResist, 85.1, 95.0);
            this.SetSkill(SkillName.Tactics, 90.1, 100.0);
            this.SetSkill(SkillName.Wrestling, 80.1, 90.0);

            this.Fame = 4000;
            this.Karma = 4000;

            this.VirtualArmor = 40;

            this.Tamable = true;
            this.ControlSlots = 1;
            this.MinTameSkill = -10;

            m_NextMysticAura = DateTime.UtcNow;
        }

        public EtherealStag(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextMysticAura)
            {
                ActivateMysticAura();
            }
        }

        private void ActivateMysticAura()
        {
            // Affect enemies
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && m.Combatant == this)
                {
                    m.Mana -= 10; // Reducing enemy mana
                    m.SendMessage("You feel a mystical energy draining your mana.");
                }
            }

            // Affect allies
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && m.Combatant != this)
                {
                    m.Mana += 5; // Increasing ally mana
                    m.SendMessage("You feel a mystical energy restoring your mana.");
                }
            }

            m_NextMysticAura = DateTime.UtcNow + TimeSpan.FromSeconds(30); // 30 seconds cooldown
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextMysticAura = DateTime.UtcNow;
        }
    }
}
