using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a black wolf corpse")]
    public class BlackWolf : BaseCreature
    {
        // Ability timers
        private DateTime m_NextHowlTime;
        private DateTime m_NextPounceTime;
        private DateTime m_NextCorruptionTime;
        private Point3D m_LastLocation;

        // Unique hue for shadowy fur
        private const int UniqueHue = 1175;

        [Constructable]
        public BlackWolf() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a black wolf";
            Body = Utility.RandomList(34, 37);
            BaseSoundID = 0xE5;
            Hue = UniqueHue;

            // — Stats —
            SetStr(300, 350);
            SetDex(150, 200);
            SetInt(80, 120);

            SetHits(800, 900);
            SetStam(200, 250);
            SetMana(0);

            SetDamage(20, 30);

            // — Damage Types —
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            // — Resistances —
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 40, 50);

            // — Skills —
            SetSkill(SkillName.Wrestling, 120.1, 130.0);
            SetSkill(SkillName.Tactics, 120.1, 130.0);
            SetSkill(SkillName.MagicResist, 100.1, 110.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;
            ControlSlots = 4;

            // Initialize cooldowns
            var now = DateTime.UtcNow;
            m_NextHowlTime       = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextPounceTime     = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCorruptionTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));

            m_LastLocation = this.Location;
        }

        // — Leave a toxic gas patch as it moves —
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (this.Map == null || !this.Alive) 
            {
                base.OnMovement(m, oldLocation);
                return;
            }

            // 25% chance per move to leave a PoisonTile
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.25)
            {
                TryCreateTile(typeof(PoisonTile), m_LastLocation);
            }

            m_LastLocation = this.Location;
            base.OnMovement(m, oldLocation);
        }

        // — Think cycle: cast abilities when off cooldown —
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // 1) Cursed Corruption Aura (poison burst around self)
            if (now >= m_NextCorruptionTime && this.InRange(Combatant.Location, 4))
            {
                CursedCorruptionAura();
                m_NextCorruptionTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            }
            // 2) Dark Pounce (teleport lunge behind target + bleed)
            else if (now >= m_NextPounceTime && this.InRange(Combatant.Location, 8))
            {
                DarkPounce();
                m_NextPounceTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // 3) Nightmare Howl (fear + stamina drain in range)
            else if (now >= m_NextHowlTime)
            {
                NightmareHowl();
                m_NextHowlTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        // — 1) Poison burst around self —
        private void CursedCorruptionAura()
        {
            this.Say("*The shadows fester!*");
            PlaySound(0x23F);

            // AoE around self
            foreach (Mobile m in Map.GetMobilesInRange(this.Location, 4))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    // Apply minor physical damage
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 100, 0, 0, 0, 0);

                    // Apply poison
                    if (m is Mobile target)
                        target.ApplyPoison(this, Poison.Greater);
                }
            }

            // Ground hazard
            TryCreateTile(typeof(PoisonTile), this.Location);
        }

        // — 2) Teleport behind and rend —
        private void DarkPounce()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) 
                return;

            // Determine spot behind target
            Point3D dest = target.Location;
            dest.X += (target.Direction == Direction.East ? -1 : target.Direction == Direction.West ? 1 : 0);
            dest.Y += (target.Direction == Direction.South ? -1 : target.Direction == Direction.North ? 1 : 0);

            if (Map.CanFit(dest, 16, false, false))
            {
                this.PlaySound(0xE5);
                this.Location = dest;
                this.ProcessDelta();

                // Heavy melee hit + bleed
                DoHarmful(target);
                AOS.Damage(target, this, Utility.RandomMinMax(50, 70), 100, 0, 0, 0, 0);
                if (target is Mobile tgt) 
                    tgt.ApplyPoison(this, Poison.Lesser);

                // Leave a landmine tile at old spot
                TryCreateTile(typeof(LandmineTile), this.Location);
            }
        }

        // — 3) Fearful howl + stamina drain —
        private void NightmareHowl()
        {
            this.Say("*ARRROOOOOWL!*");
            PlaySound(0x65A);
            FixedParticles(0x376A, 10, 20, 5032, UniqueHue, 0, EffectLayer.Head);

            // Affect all in 6-tile radius
            foreach (Mobile m in Map.GetMobilesInRange(this.Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    if (m is Mobile tgt)
                    {
                        // Stamina drain
                        int drain = Utility.RandomMinMax(20, 40);
                        tgt.Stam = Math.Max(0, tgt.Stam - drain);
                        tgt.SendMessage(0x22, "The nightmare howl exhausts you!");
                        // Fear effect: scramble their target for a moment
                        tgt.Freeze(TimeSpan.FromSeconds(2.0));
                    }
                }
            }
        }

        // — Helper to drop a ground tile of type T at a point —
        private void TryCreateTile(Type tileType, Point3D loc)
        {
            if (Map == null) return;

            if (!Map.CanFit(loc, 16, false, false))
                loc.Z = Map.GetAverageZ(loc.X, loc.Y);

            if (!Map.CanFit(loc, 16, false, false))
                return;

            var tile = (Item)Activator.CreateInstance(tileType);
            tile.Hue = UniqueHue;
            tile.MoveToWorld(loc, this.Map);
        }

        // — Death: final toxic eruption —
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*The darkness consumes...*");
                PlaySound(0x211);
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                // Spawn 4–6 toxic gas pockets
                int count = Utility.RandomMinMax(4, 6);
                for (int i = 0; i < count; i++)
                {
                    int dx = Utility.RandomMinMax(-3, 3);
                    int dy = Utility.RandomMinMax(-3, 3);
                    TryCreateTile(typeof(ToxicGasTile), new Point3D(this.X + dx, this.Y + dy, this.Z));
                }
            }

            base.OnDeath(c);
        }

        // — Loot & Properties —
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 1);
            AddLoot(LootPack.MedScrolls, 1);

            // 0.2% drop of a rare fang
            if (Utility.RandomDouble() < 0.002)
                PackItem(new BondsOfTheSpiraledVine());
        }

        public BlackWolf(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‐init timers so it's ready on server restart
            var now = DateTime.UtcNow;
            m_NextHowlTime       = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextPounceTime     = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCorruptionTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
            m_LastLocation       = this.Location;
        }
    }
}
