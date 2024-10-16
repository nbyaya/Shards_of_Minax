using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a mud wallow pig corpse")]
    public class MudWallowPig : BaseCreature
    {
        private DateTime m_NextMudWallow;
        private DateTime m_MudWallowEnd;

        [Constructable]
        public MudWallowPig()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a mud wallow pig";
            Body = 0xCB; // Using the body type from the provided example
            BaseSoundID = 0xC4;
            Hue = 1500; // Unique hue for the Mud Wallow Pig

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

            m_NextMudWallow = DateTime.UtcNow;
        }

        public MudWallowPig(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 6; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextMudWallow)
                {
                    MudWallow();
                }
            }

            if (DateTime.UtcNow >= m_MudWallowEnd && m_MudWallowEnd != DateTime.MinValue)
            {
                DeactivateMudWallow();
            }
        }

        private void MudWallow()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Mud Wallow! *");
            PlaySound(0x026);
            FixedParticles(0x374A, 10, 30, 5013, 0, 0, EffectLayer.Waist);

            SetDex(Dex + 20); // Increase evasion
            m_MudWallowEnd = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Duration of the mud wallow effect
            m_NextMudWallow = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for the ability

            // Apply slowing effect to enemies in the vicinity
            IPooledEnumerable eable = this.GetMobilesInRange(2);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m))
                {
                    m.SendMessage("You are slowed by the mud!");
                    m.Dex -= 20; // Decrease enemy dexterity
                }
            }
            eable.Free();
        }

        private void DeactivateMudWallow()
        {
            SetDex(Dex - 20); // Revert evasion boost

            // Revert the slowing effect on enemies in the vicinity
            IPooledEnumerable eable = this.GetMobilesInRange(2);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m))
                {
                    m.SendMessage("You are no longer slowed by the mud.");
                    m.Dex += 20; // Restore enemy dexterity
                }
            }
            eable.Free();

            m_MudWallowEnd = DateTime.MinValue;
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

            m_NextMudWallow = DateTime.UtcNow;
            m_MudWallowEnd = DateTime.MinValue;
        }
    }
}
