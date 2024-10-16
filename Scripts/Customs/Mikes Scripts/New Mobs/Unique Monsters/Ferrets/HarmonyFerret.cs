using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a harmony ferret corpse")]
    public class HarmonyFerret : BaseCreature
    {
        private DateTime m_NextMelodyOfPeace;
        private DateTime m_NextHarmonyEcho;
        private DateTime m_NextCrescendoOfJoy;
        private DateTime m_NextResonantAura;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public HarmonyFerret()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a harmony ferret";
            Body = 0x117; // Ferret body
            Hue = 1572; // Unique hue
			BaseSoundID = 0xCF;

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public HarmonyFerret(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}		

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextMelodyOfPeace = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextHarmonyEcho = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextCrescendoOfJoy = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextResonantAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextMelodyOfPeace)
                {
                    MelodyOfPeace();
                }

                if (DateTime.UtcNow >= m_NextHarmonyEcho)
                {
                    HarmonyEcho();
                }

                if (DateTime.UtcNow >= m_NextCrescendoOfJoy)
                {
                    CrescendoOfJoy();
                }

                if (DateTime.UtcNow >= m_NextResonantAura)
                {
                    ResonantAura();
                }
            }
        }

        private void MelodyOfPeace()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Plays a soothing tune of peace *");
            PlaySound(0x1F3); // Musical sound

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is BaseCreature && ((BaseCreature)m).IsFriend(this))
                {
                    m.Heal(15);
                    m.SendMessage("You feel a soothing wave of peace.");
                    m.AddSkillMod(new DefaultSkillMod(SkillName.MagicResist, true, m.Skills[SkillName.MagicResist].Base + 15.0));
                    m.SendMessage("Your defenses are enhanced!");
                }
            }

            m_NextMelodyOfPeace = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Adjust cooldown as needed
        }

        private void HarmonyEcho()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Creates a harmonious echo *");
            PlaySound(0x1F4); // Musical sound

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Combatant != this)
                {
                    m.SendMessage("The Harmony Ferret's echo confuses you!");
                    m.Freeze(TimeSpan.FromSeconds(3)); // Example effect
                    m.SendMessage("You are dazed and confused!");
                    m.SendMessage("Your weapon feels heavy and unwieldy!");
                    if (m is PlayerMobile player && player.FindItemOnLayer(Layer.OneHanded) != null)
                    {
                        player.SendMessage("Your weapon has been disarmed!");
                        player.EquipItem(player.FindItemOnLayer(Layer.OneHanded));
                        player.SendMessage("You have been disarmed by the Harmony Ferret's echo!");
                    }
                }
            }

            m_NextHarmonyEcho = DateTime.UtcNow + TimeSpan.FromSeconds(50); // Adjust cooldown as needed
        }

        private void CrescendoOfJoy()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Unleashes a powerful crescendo of joy *");
            PlaySound(0x1F6); // Musical burst sound

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this)
                {
                    m.Damage(20, this);
                    m.SendMessage("You are struck by a burst of musical energy!");
                    m.SendMessage("You feel your movement slowed down!");
                    m.Freeze(TimeSpan.FromSeconds(4)); // Slow down effect
                }
            }

            m_NextCrescendoOfJoy = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Adjust cooldown as needed
        }

        private void ResonantAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Creates a resonant aura *");
            PlaySound(0x1F5); // Resonant aura sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is BaseCreature && ((BaseCreature)m).IsFriend(this))
                {
                    m.SendMessage("You are empowered by the resonant aura!");
                    m.AddSkillMod(new DefaultSkillMod(SkillName.MagicResist, true, m.Skills[SkillName.MagicResist].Base + 10.0));
                }
                else if (m != this)
                {
                    m.SendMessage("You are hindered by the resonant aura!");
                    m.Damage(10, this);
                    m.SendMessage("You feel your strength sapped by the resonant aura!");
                    m.AddSkillMod(new DefaultSkillMod(SkillName.MagicResist, true, m.Skills[SkillName.MagicResist].Base - 5.0));
                }
            }

            m_NextResonantAura = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Adjust cooldown as needed
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
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
