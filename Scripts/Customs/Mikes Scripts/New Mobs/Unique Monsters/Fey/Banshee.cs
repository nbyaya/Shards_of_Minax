using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a banshee corpse")]
    public class Banshee : BaseCreature
    {
        private DateTime m_NextScreech;
        private DateTime m_NextHaunt;
        private DateTime m_NextCurse;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public Banshee()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a banshee";
            Body = 26; // GreenGoblin body
            BaseSoundID = 0x482; // Ghost sound
            Hue = 1593; // Pale blue hue

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

            m_AbilitiesInitialized = false; // Initialization flag
        }

        public Banshee(Serial serial)
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

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool AlwaysMurderer { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextScreech = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextHaunt = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextCurse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextScreech)
                {
                    Screech();
                }

                if (DateTime.UtcNow >= m_NextHaunt)
                {
                    Haunt();
                }

                if (DateTime.UtcNow >= m_NextCurse)
                {
                    Curse();
                }
            }
        }

        private void Screech()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The banshee unleashes a terrifying wail *");
            PlaySound(0x1E4); // Scream sound

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Player)
                {
                    m.SendLocalizedMessage(1008122, false, Name); // The intense horror of ~1_NAME~'s scream stops you in your tracks.
                    m.FixedParticles(0x374A, 1, 15, 9502, 97, 3, (EffectLayer)255);
                    m.PlaySound(0x210);

                    int duration = 5 + (int)(15 * (1 - ((double)m.Skills[SkillName.MagicResist].Value / 100)));
                    
                    ResistanceMod[] mods = {
                        new ResistanceMod(ResistanceType.Physical, -20),
                        new ResistanceMod(ResistanceType.Fire, -20),
                        new ResistanceMod(ResistanceType.Cold, -20),
                        new ResistanceMod(ResistanceType.Poison, -20),
                        new ResistanceMod(ResistanceType.Energy, -20)
                    };

                    TimedResistanceMod.AddMod(m, "BansheeScreech", mods, TimeSpan.FromSeconds(duration));
                }
            }

            m_NextScreech = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void Haunt()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The banshee becomes intangible *");
            PlaySound(0x1F7); // Ethereal sound

            Hidden = true;
            Blessed = true;

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                Hidden = false;
                Blessed = false;
                PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The banshee returns to corporeal form *");
            });

            m_NextHaunt = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void Curse()
        {
            List<Mobile> targets = new List<Mobile>();

            foreach (Mobile m in GetMobilesInRange(12))
            {
                if (m != this && m.Player && CanBeHarmful(m))
                {
                    targets.Add(m);
                }
            }

            if (targets.Count > 0)
            {
                Mobile target = targets[Utility.Random(targets.Count)];

                PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The banshee curses " + target.Name + " *");
                target.FixedParticles(0x374A, 10, 15, 5028, EffectLayer.Waist);
                target.PlaySound(0x1EA);

                target.AddStatMod(new StatMod(StatType.Str, "BansheeCurse_Str", -10, TimeSpan.FromSeconds(30)));
                target.AddStatMod(new StatMod(StatType.Dex, "BansheeCurse_Dex", -10, TimeSpan.FromSeconds(30)));
                target.AddStatMod(new StatMod(StatType.Int, "BansheeCurse_Int", -10, TimeSpan.FromSeconds(30)));

                target.SendLocalizedMessage(1075837); // Your life force is drained by the curse.
            }

            m_NextCurse = DateTime.UtcNow + TimeSpan.FromSeconds(45);
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
