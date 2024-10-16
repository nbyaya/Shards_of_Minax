using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a luminous jellyfish corpse")]
    public class LuminousJellyfish : BaseCreature
    {
        private DateTime m_NextLuminousHeal;
        private DateTime m_NextElectricShock;

        [Constructable]
        public LuminousJellyfish()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a luminous jellyfish";
            Body = 317; // Using jellyfish body
            BaseSoundID = 0x388; // Watery sound
            Hue = 1195; // Bright blue hue

            this.SetStr(200);
            this.SetDex(110);
            this.SetInt(150);

            this.SetDamage(14, 21);

            this.SetDamageType(ResistanceType.Physical, 0);
            this.SetDamageType(ResistanceType.Poison, 100);

            this.SetResistance(ResistanceType.Physical, 45, 55);
            this.SetResistance(ResistanceType.Fire, 50, 60);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 70, 80);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            this.SetSkill(SkillName.EvalInt, 90.1, 100.0);
            this.SetSkill(SkillName.Meditation, 90.1, 100.0);
            this.SetSkill(SkillName.Magery, 90.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 90.1, 100.0);
            this.SetSkill(SkillName.Tactics, 100.0);
            this.SetSkill(SkillName.Wrestling, 98.1, 99.0);

            this.VirtualArmor = 58;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_NextLuminousHeal = DateTime.UtcNow;
            m_NextElectricShock = DateTime.UtcNow;
        }

        public LuminousJellyfish(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 3; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextLuminousHeal)
                {
                    LuminousHeal();
                }

                if (DateTime.UtcNow >= m_NextElectricShock)
                {
                    ElectricShock();
                }
            }
        }

        private void LuminousHeal()
        {
            // Heal itself and nearby allies
            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m == this || (m is BaseCreature && ((BaseCreature)m).Controlled && ((BaseCreature)m).ControlMaster == this.ControlMaster))
                {
                    int heal = Utility.RandomMinMax(15, 25);
                    m.Heal(heal);
                    m.FixedEffect(0x376A, 9, 32);
                }
            }

            this.PlaySound(0x1E9);
            this.FixedEffect(0x37C4, 10, 36);
            m_NextLuminousHeal = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ElectricShock()
        {
            if (Combatant is Mobile)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive && target.InRange(this.Location, 2))
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
                    target.PlaySound(0x1DD);
                    target.FixedEffect(0x3818, 9, 32);

                    if (target.Body.IsHuman && !target.Mounted)
                    {
                        target.Animate(20, 7, 1, true, false, 0); // Hit animation
                    }
                }
            }

            this.PlaySound(0x1E6);
            m_NextElectricShock = DateTime.UtcNow + TimeSpan.FromSeconds(15);
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

            m_NextLuminousHeal = DateTime.UtcNow;
            m_NextElectricShock = DateTime.UtcNow;
        }
    }
}