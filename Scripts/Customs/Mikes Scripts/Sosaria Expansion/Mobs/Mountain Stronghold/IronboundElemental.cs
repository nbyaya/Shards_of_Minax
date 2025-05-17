using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("an ironbound elemental corpse")]
    public class IronboundElemental : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextMagnetTime;
        private DateTime m_NextEarthshatterTime;
        private DateTime m_NextShardVolleyTime;

        // Unique metallic hue
        private const int UniqueHue = 1175;

        [Constructable]
        public IronboundElemental()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "an ironbound elemental";
            Body = 111;            // same body as ShadowIronElemental
            BaseSoundID = 268;     // same sound
            Hue = UniqueHue;

            // --- Attributes ---
            SetStr(500, 600);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(2000, 2400);
            SetStam(300, 350);
            SetMana(0);  // no innate spellcasting

            SetDamage(25, 35);
            SetDamageType(ResistanceType.Physical, 100);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   60, 70);
            SetResistance(ResistanceType.Energy,   50, 60);

            // --- Combat Skills ---
            SetSkill(SkillName.Wrestling,    100.0, 120.0);
            SetSkill(SkillName.Tactics,      110.0, 130.0);
            SetSkill(SkillName.MagicResist,   90.0, 110.0);

            Fame = 25000;
            Karma = -25000;
            ControlSlots = 3;

            // Initialize ability cooldowns
            m_NextMagnetTime         = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextEarthshatterTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            m_NextShardVolleyTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));

            // Loot setup
            PackItem(new IronOre(Utility.RandomMinMax(20, 30)));
            PackGold(500, 800);
            PackItem(new BlueDiamond(Utility.RandomMinMax(1, 2)));
        }

        public IronboundElemental(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || !Alive)
                return;

            DateTime now = DateTime.UtcNow;

            // Magnetic Surge: pull target in and drain stamina
            if (now >= m_NextMagnetTime && this.InRange(((IEntity)Combatant).Location, 12))
            {
                TryMagneticSurge();
                m_NextMagnetTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            // Earthshatter: AoE quake + EarthquakeTile hazard
            if (now >= m_NextEarthshatterTime)
            {
                TryEarthshatter();
                m_NextEarthshatterTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            }
            // Iron Shard Volley: ranged metal shard barrage
            if (now >= m_NextShardVolleyTime)
            {
                TryShardVolley();
                m_NextShardVolleyTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 22));
            }
        }

        private void TryMagneticSurge()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("Feel the pull of iron!");
                PlaySound(0x2F3);
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                              0x376A, 8, 30, UniqueHue, 0, 5039, 0);

                // Pull the target one tile closer
                Point3D src = this.Location, dst = target.Location;
                int dx = Math.Sign(src.X - dst.X), dy = Math.Sign(src.Y - dst.Y);
                Point3D newLoc = new Point3D(dst.X + dx, dst.Y + dy, dst.Z);

                if (target.Map == this.Map && Map.CanFit(newLoc.X, newLoc.Y, newLoc.Z, 16, false, false))
                    target.MoveToWorld(newLoc, this.Map);

                // Damage + stamina drain
                DoHarmful(target);
                AOS.Damage(target, this, Utility.RandomMinMax(35, 55), 100, 0, 0, 0, 0);

                int drain = Utility.RandomMinMax(30, 50);
                if (target.Stam >= drain)
                {
                    target.Stam -= drain;
                    target.SendMessage("Your legs feel heavy from the magnetic force!");
                    target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                    target.PlaySound(0x1F8);
                }
            }
        }

        private void TryEarthshatter()
        {
            Say("*The mountain trembles!*");
            PlaySound(0x2A5);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                          0x37C4, 10, 40, UniqueHue, 0, 5026, 0);

            List<Mobile> victims = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(this.Location, 8))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    victims.Add(m);
            }

            foreach (Mobile m in victims)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(45, 65), 100, 0, 0, 0, 0);

                // Spawn a quake hazard beneath each victim
                EarthquakeTile quake = new EarthquakeTile { Hue = UniqueHue };
                quake.MoveToWorld(m.Location, this.Map);
            }
        }

        private void TryShardVolley()
        {
            Say("Shards of iron, pierce them!");
            PlaySound(0x2B1);

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(this.Location, 10))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (Mobile m in targets)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(Utility.RandomMinMax(0, 400)), () =>
                {
                    if (m != null && CanBeHarmful(m, false))
                    {
                        // Visual shard projectile
                        Effects.SendMovingParticles(
                            new Entity(Serial.Zero, this.Location, this.Map),
                            new Entity(Serial.Zero, m.Location, this.Map),
                            0x37F6, 5, 0, false, false, UniqueHue, 0, 0, 9502, 0, EffectLayer.Head, 0x100);

                        DoHarmful(m);
                        AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 100, 0, 0, 0, 0);
                    }
                });
            }
        }

        public override void OnDeath(Container c)
        {

			base.OnDeath(c);

			if (this.Map == null)
				return;

            Say("My metals... reclaim...");
            PlaySound(0x2A5);

            // Scatter landmines of metal debris
            for (int i = 0; i < Utility.RandomMinMax(4, 7); i++)
            {
                int xOff = Utility.RandomMinMax(-3, 3);
                int yOff = Utility.RandomMinMax(-3, 3);
                Point3D loc = new Point3D(X + xOff, Y + yOff, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                LandmineTile mine = new LandmineTile { Hue = UniqueHue };
                mine.MoveToWorld(loc, this.Map);
            }

            
        }

        // Immunities & properties
        public override bool BleedImmune        => true;
        public override Poison PoisonImmune     => Poison.Deadly;
        public override bool BreathImmune       => true;
        public override int TreasureMapLevel    => 5;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus      => 75.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));

            if (Utility.RandomDouble() < 0.03) // 3% chance
                PackItem(new FortressBornShinplates()); // example unique drop
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
            // Reinitialize cooldowns on load
            m_NextMagnetTime       = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextEarthshatterTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            m_NextShardVolleyTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
        }
    }
}
