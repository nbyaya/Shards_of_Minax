using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a cu-pattern vatbeast corpse")]
    public class CuPatternVatbeast : BaseCreature
    {
        private DateTime m_NextRendTime;
        private DateTime m_NextSiphonTime;
        private DateTime m_NextGlyphTime;
        private Point3D m_LastLocation;

        private const int UniqueHue = 1258; // A shifting teal‑pattern glow

        [Constructable]
        public CuPatternVatbeast() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Cù‑Pattern Vatbeast";
            Body = 277;
            Hue = UniqueHue;

            // Inherit Cu Sidhe sounds via overrides below
            BaseSoundID = 0;

            // Stats
            SetStr(1500, 1700);
            SetDex(200, 250);
            SetInt(300, 400);
            SetHits(1500, 2000);
            SetStam(200, 250);
            SetMana(800, 900);

            SetDamage(25, 35);
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold,    30);
            SetDamageType(ResistanceType.Energy,  50);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire,     30, 50);
            SetResistance(ResistanceType.Cold,     80, 90);
            SetResistance(ResistanceType.Poison,   40, 60);
            SetResistance(ResistanceType.Energy,   80, 90);

            SetSkill(SkillName.EvalInt,      110.0, 125.0);
            SetSkill(SkillName.Magery,       110.0, 125.0);
            SetSkill(SkillName.MagicResist,  115.0, 130.0);
            SetSkill(SkillName.Meditation,   100.0, 110.0);
            SetSkill(SkillName.Tactics,       95.0, 105.0);
            SetSkill(SkillName.Wrestling,     95.0, 105.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 85;
            ControlSlots = 6;
            Tamable = false;

            // Cooldowns
            var now = DateTime.UtcNow;
            m_NextRendTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSiphonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextGlyphTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));

            m_LastLocation = this.Location;

            // Loot
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new Nightshade(Utility.RandomMinMax(15, 20)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(15, 20)));
        }

        // ——————————————
        // Overridden Sounds (from Cu Sidhe)
        // ——————————————
        public override int GetIdleSound()   { return 0x577; }
        public override int GetAttackSound() { return 0x576; }
        public override int GetAngerSound()  { return 0x578; }
        public override int GetHurtSound()   { return 0x576; }
        public override int GetDeathSound()  { return 0x579; }

        // ——————————————
        // Aura: Patterned Mana Siphon (on movement)
        // ——————————————
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m == this || !Alive || m.Map != Map || !m.InRange(Location, 3))
                return;

            if (m is Mobile target && CanBeHarmful(target, false) && Utility.RandomDouble() < 0.2)
            {
                DoHarmful(target);
                int drain = Utility.RandomMinMax(5, 10);
                if (target.Mana >= drain)
                {
                    target.Mana -= drain;
                    target.SendMessage(0x22, "The vatbeast's patterns siphon your magical energy!");
                    target.FixedParticles(0x374A,  8, 12, 5032, EffectLayer.Head);
                    target.PlaySound(0x1F8);
                }
            }
        }

        // ——————————————
        // Core AI loop: trigger special attacks
        // ——————————————
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || !Alive)
                return;

            var now = DateTime.UtcNow;

            // Patterned Rend: severe single-target bleed
            if (now >= m_NextRendTime && InRange(Combatant.Location, 2))
            {
                PatternedRend();
                m_NextRendTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Glyph Rift: summons a toxic tile at the target
            else if (now >= m_NextGlyphTime && InRange(Combatant.Location, 12))
            {
                ToxicGlyphRift();
                m_NextGlyphTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));
            }
            // Mana Siphon Burst: AoE around self
            else if (now >= m_NextSiphonTime)
            {
                ManaSiphonBurst();
                m_NextSiphonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // ——————————————
        // Ability 1: Patterned Rend (single-target, heavy damage + bleed)
        // ——————————————
        public void PatternedRend()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*The patterns tear at your flesh!*");
                PlaySound(0x211);
                FixedParticles(0x3779, 10, 20, 5032, UniqueHue, 0, EffectLayer.Waist);

                DoHarmful(target);
                int damage = Utility.RandomMinMax(60, 80);
                AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);


            }
        }

        // ——————————————
        // Ability 2: Toxic Glyph Rift (places a PoisonTile at foe’s location)
        // ——————————————
        public void ToxicGlyphRift()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Ancient glyphs awaken!*");
            PlaySound(0x22F);

            var loc = target.Location;
            Effects.SendLocationParticles(
                EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                0x3728, 8, 10, UniqueHue, 0, 5039, 0
            );

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null) return;

                // Find valid ground
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var tile = new PoisonTile { Hue = UniqueHue };
                tile.MoveToWorld(loc, Map);
                Effects.PlaySound(loc, Map, 0x2F9);
            });
        }

        // ——————————————
        // Ability 3: Mana Siphon Burst (AoE around self)
        // ——————————————
        public void ManaSiphonBurst()
        {
            Say("*Feel the patterns drain you!*");
            PlaySound(0x20A);
            FixedParticles(0x3709, 12, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

            var list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    list.Add(m);
            }

            foreach (var m in list)
            {
                DoHarmful(m);
                int damage = Utility.RandomMinMax(30, 45);
                AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);

                if (m is Mobile target)
                {
                    int drain = Utility.RandomMinMax(15, 25);
                    if (target.Mana >= drain)
                    {
                        target.Mana -= drain;
                        target.SendMessage(0x22, "The vatbeast’s patterns leech your arcane lifeblood!");
                        target.FixedParticles(0x374A,  8, 12, 5032, EffectLayer.Head);
                        target.PlaySound(0x1F8);
                    }
                }
            }
        }

        // ——————————————
        // Death Explosion: spawns random hazardous tiles
        // ——————————————
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null) return;

            Say("*Patterns... unbound...!*");
            PlaySound(0x211);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0
            );

            int count = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < count; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;
                var loc = new Point3D(x, y, z);

                if (!Map.CanFit(x, y, z, 16, false, false))
                    loc.Z = Map.GetAverageZ(x, y);

                // Randomly pick between Vortex and Poison tiles
                var tile = Utility.RandomBool()
                    ? (Item)new VortexTile { Hue = UniqueHue }
                    : new PoisonTile { Hue = UniqueHue };

                tile.MoveToWorld(loc, Map);
                Effects.SendLocationParticles(
                    EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                    0x376A, 6, 20, UniqueHue, 0, 5039, 0
                );
            }
        }

        // ——————————————
        // Loot & Serialization
        // ——————————————
        public override int TreasureMapLevel => 6;
        public override bool BleedImmune   => true;
        public override double DispelDifficulty => 140.0;
        public override double DispelFocus      =>  70.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            if (Utility.RandomDouble() < 0.03) // 3% chance for a unique artifact
                PackItem(new MaxxiaScroll()); // replace with your artifact

            base.GenerateLoot();
        }

        public CuPatternVatbeast(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns
            var now = DateTime.UtcNow;
            m_NextRendTime   = now;
            m_NextSiphonTime = now;
            m_NextGlyphTime  = now;
        }
    }
}
