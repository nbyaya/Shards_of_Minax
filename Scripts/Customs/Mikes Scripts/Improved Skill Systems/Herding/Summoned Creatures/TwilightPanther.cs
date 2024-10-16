using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a twilight panther corpse")]
    public class TwilightPanther : BaseCreature
    {
        private DateTime m_NextShadowCloak;
        private DateTime m_ShadowCloakEnd;
        private bool m_IsCloaked;

        [Constructable]
        public TwilightPanther()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a twilight panther";
            Body = 0xD6; // Panther body
            BaseSoundID = 0x462; // Panther sound
            Hue = 1175; // Dark purple hue

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

            m_NextShadowCloak = DateTime.UtcNow;
            m_ShadowCloakEnd = DateTime.MinValue;
            m_IsCloaked = false;
        }

        public TwilightPanther(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Feline; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextShadowCloak && !m_IsCloaked)
                {
                    ActivateShadowCloak();
                }
            }

            if (DateTime.UtcNow >= m_ShadowCloakEnd && m_IsCloaked)
            {
                DeactivateShadowCloak();
            }
        }

        private void ActivateShadowCloak()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The twilight panther fades into the shadows *");
            PlaySound(0x22F);
            FixedEffect(0x376A, 10, 15);

            Hidden = true;
            m_IsCloaked = true;

            m_ShadowCloakEnd = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextShadowCloak = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void DeactivateShadowCloak()
        {
            Hidden = false;
            m_IsCloaked = false;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The twilight panther emerges from the shadows *");
            PlaySound(0x215);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (m_IsCloaked)
            {
                DeactivateShadowCloak();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_IsCloaked);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_IsCloaked = reader.ReadBool();

            m_NextShadowCloak = DateTime.UtcNow;
            m_ShadowCloakEnd = DateTime.MinValue;
        }
    }
}