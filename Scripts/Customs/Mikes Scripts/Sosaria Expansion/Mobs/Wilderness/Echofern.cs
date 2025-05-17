using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Spellweaving;

namespace Server.Mobiles
{
    [CorpseName("an echofern corpse")]
    public class Echofern : BaseCreature
    {
        private DateTime _NextEchoBlast;
        private DateTime _NextPhotosynthesis;
        private DateTime _NextSporeCloud;

        [Constructable]
        public Echofern()
            : base(AIType.AI_Spellweaving, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an echofern";
            Body = 0x111;
            BaseSoundID = 0x56E;
            Hue = 0x8A4;

            SetStr(500, 600);
            SetDex(150, 180);
            SetInt(600, 700);
            SetHits(800, 1000);
            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold,     40);
            SetDamageType(ResistanceType.Energy,   40);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     50, 60);
            SetResistance(ResistanceType.Cold,     50, 60);
            SetResistance(ResistanceType.Poison,   70, 80);
            SetResistance(ResistanceType.Energy,   90,100);

            SetSkill(SkillName.Wrestling,     100.0, 110.0);
            SetSkill(SkillName.Tactics,       100.0, 110.0);
            SetSkill(SkillName.MagicResist,   110.0, 120.0);
            SetSkill(SkillName.Magery,        110.0, 120.0);
            SetSkill(SkillName.EvalInt,       110.0, 120.0);
            SetSkill(SkillName.Meditation,    110.0, 120.0);
            SetSkill(SkillName.Spellweaving,  110.0, 120.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 40;

            _NextEchoBlast      = DateTime.UtcNow;
            _NextPhotosynthesis = DateTime.UtcNow;
            _NextSporeCloud     = DateTime.UtcNow;
        }

        public Echofern(Serial serial)
            : base(serial)
        {
        }

        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;

            if (combatant != null && !combatant.Deleted && combatant.Map == Map && CanBeHarmful(combatant))
            {
                // Photosynthesis
                if (Hits < HitsMax * 0.75 && DateTime.UtcNow >= _NextPhotosynthesis)
                {
                    PerformPhotosynthesis();
                    _NextPhotosynthesis = DateTime.UtcNow + TimeSpan.FromSeconds(45.0);
                }

                // Echo Blast
                if (InRange(combatant, 8) && InLOS(combatant) && DateTime.UtcNow >= _NextEchoBlast)
                {
                    InitiateEchoBlast();
                    _NextEchoBlast = DateTime.UtcNow + TimeSpan.FromSeconds(17.0);
                }

                // Spore Cloud
                if (DateTime.UtcNow >= _NextSporeCloud)
                {
                    ReleaseSporeCloud();
                    _NextSporeCloud = DateTime.UtcNow + TimeSpan.FromSeconds(20.0 + (Utility.RandomDouble() * 10.0));
                }
            }

            base.OnActionCombat();
        }

        // ─── Echo Blast ───────────────────────────────────────────────────────────────

        private const double EchoBlastWindup = 2.0;
        private const int    EchoBlastRange  = 6;

        public void InitiateEchoBlast()
        {
            // moving "hum" effect on itself
            Effects.SendMovingParticles(
                this, this,                // from, to
                0x37C4, 1, 0,              // itemID, speed, duration
                false, false,              // fixedDir, explodes
                0x8A4, 0,                  // hue, renderMode
                9502, 6014, 0x1F3,         // effect, explodeEffect, explodeSound
                EffectLayer.CenterFeet,    // layer
                0                          // unknown
            );

            PlaySound(0x65A);
            PublicOverheadMessage(MessageType.Regular, Hue, false, "*The echofern hums with building energy!*");

            Timer.DelayCall(TimeSpan.FromSeconds(EchoBlastWindup), ExecuteEchoBlast);
        }

        public void ExecuteEchoBlast()
        {
            PlaySound(0x207);

            // ripple effect at its own location
/* 			Effects.SendMovingParticles(
				this, this,               // from, to
				0x37C4,                   // itemID
				TimeSpan.Zero,            // ← new delay parameter
				1, 0,                     // speed, duration
				false, false,             // fixedDir, explodes
				0x8A4, 0,                 // hue, renderMode
				9502, 6014, 0x1F3,        // effect, explodeEffect, explodeSound
				EffectLayer.CenterFeet,   // layer
				0                         // unknown
			); */

            var targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, EchoBlastRange))
            {
                if (m != this && CanBeHarmful(m))
                    targets.Add(m);
            }

            foreach (Mobile t in targets)
            {
                DoHarmful(t);

                int dmg = Utility.RandomMinMax(30, 50);
                AOS.Damage(t, this, dmg, 0, 0, 40, 0, 60);

                int mp = Utility.RandomMinMax(20, 30);
                if (t.Mana > 0)
                {
                    t.Mana = Math.Max(0, t.Mana - mp);
                    t.SendMessage("The echoing wave drains your mana!");
                }

                Effects.SendTargetParticles(
                    t,
                    0x37CC, 1, 40,            // itemID, speed, duration
                    0x8A4, 0,                 // hue, renderMode
                    9502,                     // effect
                    EffectLayer.Waist,        // layer
                    0                         // unknown
                );
            }
        }

        // ─── Melee Drain ─────────────────────────────────────────────────────────────

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() <= 0.30 && defender is Mobile m)
            {
                int sd = Utility.RandomMinMax(30, 50);
                if (m.Stam > 0)
                {
                    m.Stam = Math.Max(0, m.Stam - sd);
                    m.SendMessage("The echofern's touch drains your stamina!");
                }

                int md = Utility.RandomMinMax(10, 20);
                if (m.Mana > 0)
                {
                    m.Mana = Math.Max(0, m.Mana - md);
                    m.SendMessage("The echofern's touch saps your energy!");
                }

                Effects.SendTargetParticles(
                    m,
                    0x37B9, 1, 40,
                    0x8A4, 0,
                    9502,
                    EffectLayer.Waist,
                    0
                );

                PlaySound(0x232);
            }
        }

        // ─── Photosynthesis ─────────────────────────────────────────────────────────

        public void PerformPhotosynthesis()
        {
            PlaySound(0x1EE);
            PublicOverheadMessage(MessageType.Regular, Hue, false, "*The echofern absorbs ambient energy!*");

/* 			Effects.SendLocationParticles(
				EffectItem.Create(new Point3D(X, Y, Z), Map, 0),
				0x37CD,
				TimeSpan.Zero,  // ← here
				1, 20,
				0x8A4, 0,
				9502, 0
			); */

            int heal = Utility.RandomMinMax(75, 125);
            Hits = Math.Min(HitsMax, Hits + heal);
        }

        // ─── Spore Cloud ────────────────────────────────────────────────────────────

        private const int SporeRange = 5;

        public void ReleaseSporeCloud()
        {
            PlaySound(0x5A1);
            PublicOverheadMessage(MessageType.Regular, Hue, false, "*A cloud of disorienting spores erupts!*");

/* 			Effects.SendTargetParticles(
				t,
				0x37CC,
				TimeSpan.Zero,  // ← and here
				1, 40,
				0x8A4, 0,
				9502,
				EffectLayer.Waist,
				0
			); */

            var targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, SporeRange))
            {
                if (m != this && CanBeHarmful(m))
                    targets.Add(m);
            }

            foreach (Mobile t in targets)
            {
                DoHarmful(t);

                // Debuff every skill by 10 for 5 seconds
				// use the Skill.SkillName property instead :contentReference[oaicite:0]{index=0}
				foreach (Skill sk in t.Skills)
				{
					t.AddSkillMod(new TimedSkillMod(
						sk.SkillName,                        // ← FIXED: enum directly on Skill
						false,
						10.0,
						TimeSpan.FromSeconds(5.0)
					));
				}
                t.SendMessage("The spores disorient you, reducing your skills!");

                Effects.SendTargetParticles(
                    t,
                    0x37B9, 1, 40,
                    Hue, 0,
                    9502,
                    EffectLayer.Waist,
                    0
                );
            }
        }

        // ─── Loot ───────────────────────────────────────────────────────────────────

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems);
            AddLoot(LootPack.Potions);
        }

        // ─── Serialization ──────────────────────────────────────────────────────────

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            _NextEchoBlast      = DateTime.UtcNow;
            _NextPhotosynthesis = DateTime.UtcNow;
            _NextSporeCloud     = DateTime.UtcNow;
        }

        public override bool CanRummageCorpses => false;
        public override int TreasureMapLevel  => 4;
        public override int Meat              => 0;
        public override int Scales            => 0;
    }
}
