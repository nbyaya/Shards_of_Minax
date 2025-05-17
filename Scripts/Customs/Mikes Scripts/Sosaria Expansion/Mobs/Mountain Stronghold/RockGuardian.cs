using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a stone guardian corpse")]
    public class RockGuardian : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextStompTime;
        private DateTime m_NextBarrageTime;
        private DateTime m_NextQuakeTime;
        private DateTime m_NextPetrifyTime;

        // Unique earth‑tone hue
        private const int UniqueHue = 2213;

        [Constructable]
        public RockGuardian() 
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Rock Guardian";
            Body = 722;
            Hue  = UniqueHue;

            // --- Stats ---
            SetStr(600, 700);
            SetDex(100, 150);
            SetInt(50,  80);

            SetHits(2000, 2500);
            SetStam(150,  200);
            SetMana(0);

            SetDamage(20, 30);

            // --- Damage Types ---
            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Cold,     10);
            SetDamageType(ResistanceType.Fire,     10);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     20, 30);
            SetResistance(ResistanceType.Poison,   40, 50);
            SetResistance(ResistanceType.Energy,   30, 40);

            // --- Skills ---
            SetSkill(SkillName.Tactics,      120.0, 140.0);
            SetSkill(SkillName.Wrestling,    120.0, 140.0);
            SetSkill(SkillName.MagicResist,   80.0, 100.0);

            Fame        = 18000;
            Karma       = -18000;
            VirtualArmor = 60;
            ControlSlots = 4;

            // Initialize our ability timers
            var now = DateTime.UtcNow;
            m_NextStompTime   = now + TimeSpan.FromSeconds(10 + Utility.RandomDouble() * 5);
            m_NextBarrageTime = now + TimeSpan.FromSeconds(15 + Utility.RandomDouble() * 10);
            m_NextQuakeTime   = now + TimeSpan.FromSeconds(20 + Utility.RandomDouble() * 10);
            m_NextPetrifyTime = now + TimeSpan.FromSeconds(12 + Utility.RandomDouble() * 8);

            // --- Loot ---
            PackGold(500, 800);
            PackItem(new IronOre(Utility.RandomMinMax(15, 25)));
            PackGem();
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null)
                return;

            var now = DateTime.UtcNow;

            // 1) Earthquake Stomp — AoE burst + tile
            if (now >= m_NextStompTime && InRange(Combatant.Location, 6))
            {
                EarthquakeStomp();
                m_NextStompTime = now + TimeSpan.FromSeconds(15 + Utility.RandomDouble() * 10);
            }

            // 2) Rockslide Barrage — landmines around target
            if (now >= m_NextBarrageTime)
            {
                RockslideBarrage();
                m_NextBarrageTime = now + TimeSpan.FromSeconds(20 + Utility.RandomDouble() * 15);
            }

            // 3) Focused Quake — a quicksand tile beneath the combatant
            if (now >= m_NextQuakeTime)
            {
                FocusedQuake();
                m_NextQuakeTime = now + TimeSpan.FromSeconds(25 + Utility.RandomDouble() * 15);
            }

            // 4) Petrifying Gaze — single‑target slow/stam‑drain
            if (now >= m_NextPetrifyTime && InRange(Combatant.Location, 8))
            {
                if (Combatant is Mobile target && CanBeHarmful(target, false))
                {
                    PetrifyingGaze(target);
                    m_NextPetrifyTime = now + TimeSpan.FromSeconds(30 + Utility.RandomDouble() * 20);
                }
            }
        }

        // Passive aura: melee aurasdeal minor physical when they stumble close
        public override void OnMovement(Mobile m, Point3D oldLoc)
        {
            base.OnMovement(m, oldLoc);

            if (m != this && Alive && m.Map == this.Map && m.InRange(Location, 3) && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    AOS.Damage(target, this, 10, 100, 0, 0, 0, 0);
                    target.SendMessage("The ground heaves and slams you!");
                }
            }
        }

        // --- Special Abilities ---

        private void EarthquakeStomp()
        {
            PlaySound(0x550);
            FixedParticles(0x376A, 10, 30, 5052, UniqueHue, 0, EffectLayer.Waist);

            var list = Map.GetMobilesInRange(Location, 5);
            foreach (Mobile m in list)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 100, 0, 0, 0, 0);

                    // Spawn an earthquake tile under them
                    if (Map != null)
                    {
                        var tile = new EarthquakeTile();
                        tile.MoveToWorld(m.Location, Map);
                    }
                }
            }
            list.Free();
        }

        private void RockslideBarrage()
        {
            Say("*The mountain itself turns against you!*");
            PlaySound(0x3E9);

            int mines = Utility.RandomMinMax(3, 6);
            for (int i = 0; i < mines; i++)
            {
                int xOff = Utility.RandomMinMax(-4, 4);
                int yOff = Utility.RandomMinMax(-4, 4);

                var loc = new Point3D(
                    Combatant.Location.X + xOff,
                    Combatant.Location.Y + yOff,
                    Combatant.Location.Z
                );
                loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var mine = new LandmineTile();
                mine.Hue = UniqueHue;
                mine.MoveToWorld(loc, Map);
            }
        }

        private void FocusedQuake()
        {
            Say("*Crush beneath stone!*");
            PlaySound(0x3B3);

            if (Combatant is Mobile m && CanBeHarmful(m, false))
            {
                var loc = m.Location;
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var qs = new QuicksandTile();
                qs.MoveToWorld(loc, Map);
            }
        }

        private void PetrifyingGaze(Mobile target)
        {
            DoHarmful(target);
            target.SendMessage("You feel your limbs stiffen under the Guardian’s gaze!");
            target.FixedParticles(0x376A, 5, 10, 5032, UniqueHue, 0, EffectLayer.Head);
            target.PlaySound(0x4F3);

            // Stam drain as “petrify” effect
            int drain = Utility.RandomMinMax(20, 40);
            target.Stam = Math.Max(0, target.Stam - drain);
        }

        // --- Death Effect ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

			if (this.Map == null)
				return;

            PlaySound(0x550);
            for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
            {
                int x = X + Utility.RandomMinMax(-2, 2);
                int y = Y + Utility.RandomMinMax(-2, 2);
                int z = Map.GetAverageZ(x, y);

                var lava = new HotLavaTile();
                lava.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // --- Sounds (reuse your UndeadGuardian ones) ---
        public override int GetIdleSound()  => 1609;
        public override int GetAngerSound() => 1606;
        public override int GetHurtSound()  => 1608;
        public override int GetDeathSound() => 1607;

        // --- Loot & Misc ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 10));
        }

        public override int TreasureMapLevel => 5;
        public override bool BleedImmune    => true;
        public override double DispelDifficulty => 120.0;
        public override double DispelFocus      => 60.0;

        public RockGuardian(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑init timers
            var now = DateTime.UtcNow;
            m_NextStompTime   = now + TimeSpan.FromSeconds(10 + Utility.RandomDouble() * 5);
            m_NextBarrageTime = now + TimeSpan.FromSeconds(15 + Utility.RandomDouble() * 10);
            m_NextQuakeTime   = now + TimeSpan.FromSeconds(20 + Utility.RandomDouble() * 10);
            m_NextPetrifyTime = now + TimeSpan.FromSeconds(12 + Utility.RandomDouble() * 8);
        }
    }
}
