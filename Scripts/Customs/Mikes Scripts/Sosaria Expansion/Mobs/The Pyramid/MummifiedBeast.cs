using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a mummified beast corpse")]
    public class MummifiedBeast : BaseCreature
    {
        // Ability cooldown timers
        private DateTime _nextCurseTime;
        private DateTime _nextSandstormTime;
        private DateTime _nextSummonTime;
        private Point3D _lastLocation;

        // Unique sand‑gold hue
        private const int UniqueHue = 2600;

        [Constructable]
        public MummifiedBeast()
            : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
        {
            Name = "a mummified beast";
            Body = 775;               // same body as plague beast
            BaseSoundID = 0x1BF;      // same idle sound
            Hue = UniqueHue;          // golden, sand‑washed tone

            // Stats
            SetStr(350, 450);
            SetDex(120, 150);
            SetInt(100, 120);

            SetHits(500, 700);
            SetStam(200, 250);
            SetMana(100, 150);

            SetDamage(25, 30);

            // Damage types
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     30, 40);
            SetResistance(ResistanceType.Poison,   70, 80);
            SetResistance(ResistanceType.Energy,   30, 40);

            // Skills
            SetSkill(SkillName.Tactics,       120.0, 130.0);
            SetSkill(SkillName.Wrestling,     120.0, 130.0);
            SetSkill(SkillName.MagicResist,   120.0, 130.0);
            SetSkill(SkillName.Anatomy,       100.0, 110.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 60;
            ControlSlots = 4;

            // Initialize cooldowns
            _nextCurseTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            _nextSandstormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            _nextSummonTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));

            // Loot
            PackGold(500, 1000);
            PackItem(new MaxxiaScroll());          // custom pyramid scroll
            PackItem(new Bone(Utility.RandomMinMax(2, 5)));
            if (Utility.RandomDouble() < 0.05)
                PackItem(new Glimmerchant());    // 5% chance unique artifact

            _lastLocation = this.Location;
        }

        // Aura: anyone moving near leaves behind quicksand hazards
        public override void OnMovement( Mobile m, Point3D oldLocation )
        {
            base.OnMovement(m, oldLocation);

            if ( m != this && m.Alive && m.Map == this.Map && m.InRange(Location, 2) && CanBeHarmful(m, false) )
            {
                // drop a quicksand tile under them
                if ( Utility.RandomDouble() < 0.25 )
                {
                    var qs = new QuicksandTile();
                    qs.MoveToWorld(m.Location, Map);
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if ( Combatant == null || !Alive || Map == null || Map == Map.Internal )
                return;

            var now = DateTime.UtcNow;

            if ( now >= _nextCurseTime && InRange(Combatant.Location, 8) )
            {
                CastBindingCurse();
                _nextCurseTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            else if ( now >= _nextSandstormTime )
            {
                UnleashSandstorm();
                _nextSandstormTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
            }
            else if ( now >= _nextSummonTime && Hits < HitsMax * 0.75 )
            {
                SummonMummySpawn();
                _nextSummonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // --- Ability 1: Binding Curse ---
        private void CastBindingCurse()
        {
            if (!(Combatant is Mobile target)) return;

            Say("*By the Pharaoh’s will, be bound!*");
            target.Freeze(TimeSpan.FromSeconds(4));            // roots target in place
            target.Stam = Math.Max(0, target.Stam -  Utility.RandomMinMax(20, 40));
            target.SendMessage("Sand coils around your legs, slowing you!");
            target.FixedParticles(0x3779, 10, 20, 5032, UniqueHue, 0, EffectLayer.Head);
            PlaySound(0x1C0);
        }

        // --- Ability 2: Sandstorm (AoE ground hazard) ---
        private void UnleashSandstorm()
        {
            Say("*The sands rise!*");
            PlaySound(0x229);
            Point3D center = this.Location;

            for ( int dx = -3; dx <= 3; dx++ )
            for ( int dy = -3; dy <= 3; dy++ )
            {
                var loc = new Point3D(center.X + dx, center.Y + dy, center.Z);
                if ( Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false) && Utility.RandomDouble() < 0.6 )
                {
                    var tile = new QuicksandTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, Map);
                }
            }
        }

        // --- Ability 3: Summon Minor Mummies when wounded ---
        private void SummonMummySpawn()
        {
            Say("*Rise from the sands!*");
            PlaySound(0x1C2);

            int count = Utility.RandomMinMax(1, 3);
            for ( int i = 0; i < count; i++ )
            {
                var spawn = new Mummy();    // a smaller mummy defined elsewhere
                spawn.Team = Team;
                spawn.MoveToWorld(Location, Map);
                spawn.Combatant = Combatant;
            }
        }

        public override void OnGaveMeleeAttack( Mobile defender )
        {
            base.OnGaveMeleeAttack(defender);

            // Chance to spray poison on melee hit
            if ( Utility.RandomDouble() < 0.2 && defender is Mobile tgt )
            {
                tgt.ApplyPoison( this, Poison.Lethal );
                tgt.SendMessage("The mummified beast’s toxic ichor coats your flesh!");
                tgt.FixedParticles(0x3779, 10, 15, 5030, UniqueHue, 0, EffectLayer.Waist);
            }
        }

        // --- Death effect: toxic gas vents poison tiles ---
        public override void OnDeath( Container c )
        {
            if ( Map != null )
            {
                Say("*My curse endures...*");
                PlaySound(0x207);

                for ( int i = 0; i < 6; i++ )
                {
                    int dx = Utility.RandomMinMax(-4, 4), dy = Utility.RandomMinMax(-4, 4);
                    var loc = new Point3D(X + dx, Y + dy, Z);
                    if ( Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false) )
                    {
                        var tile = new PoisonTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld(loc, Map);
                        Effects.SendLocationParticles(
                            EffectItem.Create( loc, Map, TimeSpan.FromSeconds(2) ),
                            0x36BD, 8, 20, UniqueHue, 0, 5026, 0 );
                    }
                }
            }

            base.OnDeath(c);
        }

        // Standard overrides & loot
        public override bool BleedImmune   => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 6;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(1, 2));
        }

        // Serialization
        public MummifiedBeast( Serial serial ) : base( serial ) { }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns on reload
            _nextCurseTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            _nextSandstormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            _nextSummonTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
        }
    }
}
