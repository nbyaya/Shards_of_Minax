using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;           // for spell effects
using Server.Spells.Seventh;   // for chain logic if desired

namespace Server.Mobiles
{
    [CorpseName("the scorched remains of the Flame of the Nile")]
    public class FlameOfTheNile : BaseCreature
    {
        // Ability cooldowns
        private DateTime m_NextTorrent;
        private DateTime m_NextFissure;
        private DateTime m_NextScarab;
        private Point3D m_LastLocation;

        // Unique golden‑orange hue
        private const int UniqueHue = 1353;

        [Constructable]
        public FlameOfTheNile()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Flame of the Nile";
            Title = "Avatar of Sand and Fire";
            Body = 0x190;                    // human form
            BaseSoundID = 0x1E9;             // human mage sound
            Hue = UniqueHue;

            // Stats
            SetStr(380, 420);
            SetDex(250, 290);
            SetInt(450, 500);

            SetHits(1200, 1400);
            SetStam(200, 250);
            SetMana(500, 600);

            SetDamage(20, 25);

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Fire, 80);
            SetDamageType(ResistanceType.Cold, 10);
            SetDamageType(ResistanceType.Poison, 0);
            SetDamageType(ResistanceType.Energy, 10);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 100.1, 110.0);
            SetSkill(SkillName.Magery, 100.1, 110.0);
            SetSkill(SkillName.MagicResist, 120.2, 130.0);
            SetSkill(SkillName.Meditation, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 80.1, 90.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 70;
            ControlSlots = 4;

            // Initialize cooldowns
            m_NextTorrent = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextFissure = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextScarab  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));

            m_LastLocation = this.Location;

            // Base loot
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 25)));
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 25)));
            PackItem(new Nightshade(Utility.RandomMinMax(15, 25)));
        }

        // Leave hot lava in its wake
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Deleted || this.Map == null) return;

            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.15)
            {
                var loc = m_LastLocation;
                m_LastLocation = this.Location;

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var lava = new HotLavaTile();
                lava.Hue = UniqueHue;
                lava.MoveToWorld(loc, this.Map);
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == Map.Internal) return;

            // Searing Torrent: long‑range fire column
            if (DateTime.UtcNow >= m_NextTorrent && InRange(Combatant.Location, 12))
            {
                SearingTorrent();
                m_NextTorrent = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Sand Fissure: melee‑range quake
            else if (DateTime.UtcNow >= m_NextFissure && InRange(Combatant.Location, 4))
            {
                SandFissure();
                m_NextFissure = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Scarab Swarm: mid‑range poison hazards
            else if (DateTime.UtcNow >= m_NextScarab && InRange(Combatant.Location, 8))
            {
                ScarabSwarm();
                m_NextScarab = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
        }

        // --- Ability 1: Searing Torrent ---
        private void SearingTorrent()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            Say("*Behold the Nile's inferno!*");
            PlaySound(0x208); // Fireball

            var loc = target.Location;
            Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                0x3709, 20, 30, UniqueHue, 0, 5032, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null) return;

                var flame = new FlamestrikeHazardTile();
                flame.Hue = UniqueHue;
                flame.MoveToWorld(loc, Map);
            });
        }

        // --- Ability 2: Sand Fissure (AoE quake + stun) ---
        private void SandFissure()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            Say("*The sands shall swallow you!*");
            PlaySound(0x21F); // Rock crack

            var center = this.Location;
            Effects.SendLocationParticles(EffectItem.Create(center, Map, EffectItem.DefaultDuration),
                0x376A, 10, 60, UniqueHue, 0, 5039, 0);

            IPooledEnumerable eable = Map.GetMobilesInRange(center, 5);
            foreach (Mobile m in eable)
            {
                if (m == this || !CanBeHarmful(m, false)) continue;
                DoHarmful(m);

                AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 100, 0, 0, 0, 0);
                m.Stam -= Utility.RandomMinMax(20, 40);
                m.Freeze(TimeSpan.FromSeconds(2.0));

                m.SendMessage("The earth trembles beneath your feet!");
            }
            eable.Free();
        }

        // --- Ability 3: Scarab Swarm (Poison‑tile burst) ---
        private void ScarabSwarm()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            Say("*Rise, my desert legions!*");
            PlaySound(0x2F3); // insect stir

            var loc = target.Location;
            Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                0x374A, 10, 30, 0, 0, 5032, 0);

            // create multiple poison tiles around target
            for (int i = 0; i < 4; i++)
            {
                int dx = Utility.RandomMinMax(-2, 2);
                int dy = Utility.RandomMinMax(-2, 2);
                var tileLoc = new Point3D(loc.X + dx, loc.Y + dy, loc.Z);

                if (!Map.CanFit(tileLoc.X, tileLoc.Y, tileLoc.Z, 16, false, false))
                    tileLoc.Z = Map.GetAverageZ(tileLoc.X, tileLoc.Y);

                var poison = new PoisonTile();
                poison.Hue = UniqueHue;
                poison.MoveToWorld(tileLoc, Map);
            }
        }

        // Death: ring of fire + poison hazards
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*The Nile... reclaims me...*");
                PlaySound(0x211);
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 20, 60, UniqueHue, 0, 5052, 0);

                for (int i = 0; i < 6; i++)
                {
                    int dx = Utility.RandomMinMax(-3, 3);
                    int dy = Utility.RandomMinMax(-3, 3);
                    var loc = new Point3D(X + dx, Y + dy, Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var flame = new FlamestrikeHazardTile();
                    flame.Hue = UniqueHue;
                    flame.MoveToWorld(loc, Map);

                    var poison = new PoisonTile();
                    poison.Hue = UniqueHue;
                    poison.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        // Loot & properties
        public override bool BleedImmune      => true;
        public override int  TreasureMapLevel => 6;
        public override double DispelDifficulty => 135.0;
        public override double DispelFocus      => 65.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(2, 3));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new DrakkonsRootedStance());    // unique artifact
        }

        public FlameOfTheNile(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑init cooldowns
            m_NextTorrent = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextFissure = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextScarab  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
        }
    }
}
