using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("an anthrax rat corpse")]
    public class AnthraxRat : BaseCreature
    {
        private DateTime m_NextSporeBurst;
        private DateTime m_NextAcidicSpit;
        private DateTime m_NextFrenzy;
        private bool m_AbilitiesInitialized; // Flag to check if abilities are initialized

        [Constructable]
        public AnthraxRat()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an anthrax rat";
            Body = 0xD7; // GiantRat body
            Hue = 2271; // Unique hue
            this.BaseSoundID = 0xCC;
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

            // Initialize ability cooldowns
            m_AbilitiesInitialized = false; 
        }

        public AnthraxRat(Serial serial)
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
                    m_NextSporeBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextAcidicSpit = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextFrenzy = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 120));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSporeBurst)
                {
                    SporeBurst();
                }

                if (DateTime.UtcNow >= m_NextAcidicSpit)
                {
                    AcidicSpit();
                }

                if (DateTime.UtcNow >= m_NextFrenzy)
                {
                    Frenzy();
                }
            }
        }

        private void SporeBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Deadly spores burst from the Anthrax Rat, poisoning all around!*");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    if (m is PlayerMobile)
                    {
                        PlayerMobile player = (PlayerMobile)m;
                        int damage = Utility.RandomMinMax(10, 15);
                        player.Damage(damage, this);
                        player.SendMessage("You are engulfed by a cloud of poisonous spores!");
                        player.SendMessage("Your resistance to poison has been reduced!");

                        // Reduce poison resistance
                        player.AddStatMod(new StatMod(StatType.Str, "SporeBurstStr", -10, TimeSpan.FromSeconds(30)));
                        player.AddStatMod(new StatMod(StatType.Dex, "SporeBurstDex", -10, TimeSpan.FromSeconds(30)));
                        player.AddStatMod(new StatMod(StatType.Int, "SporeBurstInt", -10, TimeSpan.FromSeconds(30)));

                        Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                        {
                            if (player != null && !player.Deleted)
                            {
                                player.SendMessage("Your poison resistance has returned to normal.");
                            }
                        });
                    }
                }
            }

            m_NextSporeBurst = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for SporeBurst
        }

        private void AcidicSpit()
        {
            Mobile target = Combatant as Mobile; // Safely cast Combatant to Mobile
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Anthrax Rat spits a glob of corrosive acid! *");
                Effects.SendTargetEffect(target, 0x3709, 16); // Acid splash effect
                int damage = Utility.RandomMinMax(10, 20);
                target.Damage(damage, this);
                target.SendMessage("You are hit by a glob of corrosive acid!");

                Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                {
                    if (target.Alive)
                    {
                        target.SendMessage("The acid burns away, leaving a painful sting.");
                    }
                });
            }

            m_NextAcidicSpit = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for AcidicSpit
        }

        private void Frenzy()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Anthrax Rat goes into a frenzied rage!*");
            PlaySound(0x165); // Frenzy sound effect
            FixedEffect(0x37C4, 10, 36); // Frenzy visual effect

            SetStr(Str + 20);
            SetDex(Dex + 20);
            SetInt(Int + 10);

            Timer.DelayCall(TimeSpan.FromSeconds(15), () =>
            {
                if (!Deleted)
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Anthrax Rat calms down.*");
                    SetStr(Str - 20);
                    SetDex(Dex - 20);
                    SetInt(Int - 10);
                }
            });

            m_NextFrenzy = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for Frenzy
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

            // Reset the ability initialization flag on deserialization
            m_AbilitiesInitialized = false;
        }
    }
}
