using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;
using Server.Spells.First; // Ensure this namespace is included for spell casting

namespace Server.Mobiles
{
    [CorpseName("a nefertiti corpse")]
    public class Nefertiti : BaseCreature
    {
        private DateTime m_NextEnchantedDance;
        private DateTime m_NextMirrorOfBeauty;
        private DateTime m_NextCurseOfThePharaoh;
        private DateTime m_NextSummonSandWraiths;
        private bool m_IsMirrorActive;

        [Constructable]
        public Nefertiti()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Nefertiti the Enchantress";
            Body = 154; // Mummy body
            Hue = 2162; // Unique hue for Nefertiti
			BaseSoundID = 471;

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

            // Initialize abilities
            Random rand = new Random();
            m_NextEnchantedDance = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
            m_NextMirrorOfBeauty = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
            m_NextCurseOfThePharaoh = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
            m_NextSummonSandWraiths = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 70));
        }

        public Nefertiti(Serial serial)
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
                if (DateTime.UtcNow >= m_NextEnchantedDance)
                {
                    EnchantedDance();
                }

                if (DateTime.UtcNow >= m_NextMirrorOfBeauty)
                {
                    MirrorOfBeauty();
                }

                if (DateTime.UtcNow >= m_NextCurseOfThePharaoh)
                {
                    CurseOfThePharaoh();
                }

                if (DateTime.UtcNow >= m_NextSummonSandWraiths)
                {
                    SummonSandWraiths();
                }
            }
        }

        private void EnchantedDance()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Nefertiti performs a mesmerizing dance! *");
            PlaySound(0x1D3); // Dance sound effect

            // Dance visual effect
            FixedParticles(0x373A, 9, 32, 5030, EffectLayer.Waist);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.SendMessage("You are entranced by Nefertiti's dance and attack your allies!");
                   

                    // Additional effects: disarm and stun
                    if (Utility.RandomDouble() < 0.3) // 30% chance to disarm
                    {
                        m.SendMessage("You are disarmed by the enchantment!");
                        m.SendMessage("You are stunned by the enchantment!");
                        // Replace with appropriate gump if necessary
                    }
                }
            }

            m_NextEnchantedDance = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void MirrorOfBeauty()
        {
            if (!m_IsMirrorActive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Nefertiti is surrounded by a reflective shimmer! *");
                PlaySound(0x1D3); // Shimmer sound effect

                // Mirror effect visual
                FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);

                m_IsMirrorActive = true;
                Timer.DelayCall(TimeSpan.FromSeconds(10), () => m_IsMirrorActive = false); // Mirror effect lasts for 10 seconds
                m_NextMirrorOfBeauty = DateTime.UtcNow + TimeSpan.FromSeconds(40);
            }
        }

        private void CurseOfThePharaoh()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Nefertiti curses her enemy with the wrath of the Pharaoh! *");
                PlaySound(0x1D3); // Curse sound effect

                // Curse effect visual
                FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);

                int curseDamage = Utility.RandomMinMax(20, 40);
                int statReduction = 20; // Reduces stats by 20
                
                if (Combatant is Mobile target)
                {
                    target.SendMessage("You are cursed by Nefertiti's dark magic!");
                    AOS.Damage(target, this, curseDamage, 0, 0, 100, 0, 0); // Curse damage

                    target.Dex -= statReduction;
                    target.Str -= statReduction;
                    target.Int -= statReduction;

                    Timer.DelayCall(TimeSpan.FromSeconds(10), () => {
                        if (target != null)
                        {
                            target.Dex += statReduction;
                            target.Str += statReduction;
                            target.Int += statReduction;
                        }
                    });
                }

                m_NextCurseOfThePharaoh = DateTime.UtcNow + TimeSpan.FromSeconds(60);
            }
        }

        private void SummonSandWraiths()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Nefertiti summons the Sand Wraiths to her aid! *");
            PlaySound(0x1D3); // Summoning sound effect

            // Summon Sand Wraiths
            for (int i = 0; i < 2; i++) // Summon 2 Sand Wraiths
            {
                SandWraith wraith = new SandWraith();
                wraith.MoveToWorld(Location, Map);
            }

            m_NextSummonSandWraiths = DateTime.UtcNow + TimeSpan.FromSeconds(70);
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (m_IsMirrorActive)
            {
                int reflectedDamage = (int)(damage * 0.25); // Reflect 25% of the damage
                from.SendMessage("Nefertiti reflects some of the damage back at you!");
                damage -= reflectedDamage;
                AOS.Damage(from, this, reflectedDamage, 0, 0, 100, 0, 0); // Reflect damage

                // Additional effect: inflict a weakness
                if (Utility.RandomDouble() < 0.3) // 30% chance to apply weakness
                {
                    from.SendMessage("You feel weakened by the reflection!");
                    from.Dex -= 10;
                    from.Str -= 10;
                    from.Int -= 10;
                    Timer.DelayCall(TimeSpan.FromSeconds(10), () => {
                        if (from != null)
                        {
                            from.Dex += 10;
                            from.Str += 10;
                            from.Int += 10;
                        }
                    });
                }
            }
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

    public class SandWraith : BaseCreature
    {
        [Constructable]
        public SandWraith() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Sand Wraith";
            Body = 64; // Example wraith body
            Hue = 0x4001; // Example hue

            SetStr(200, 250);
            SetDex(120, 140);
            SetInt(150, 200);

            SetHits(150, 200);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 70.0, 90.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 2000;
            Karma = -2000;

            VirtualArmor = 50;

            // Initialize abilities
        }

        public SandWraith(Serial serial) : base(serial)
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
