import { useEffect, useState } from 'react'
import { getServices } from './api'
import type { ServiceOffering } from './types'
import './App.css'

const currencyFormatter = new Intl.NumberFormat('en-US', {
  style: 'currency',
  currency: 'USD',
  minimumFractionDigits: 0,
})

function App() {
  const [services, setServices] = useState<ServiceOffering[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [requestCount, setRequestCount] = useState(0)

  useEffect(() => {
    let ignore = false

    getServices()
      .then((serviceData) => {
        if (!ignore) {
          setServices(serviceData)
        }
      })
      .catch((requestError: Error) => {
        if (!ignore) {
          setError(requestError.message)
        }
      })
      .finally(() => {
        if (!ignore) {
          setLoading(false)
        }
      })

    return () => {
      ignore = true
    }
  }, [requestCount])

  const retryRequest = () => {
    setLoading(true)
    setError('')
    setRequestCount((count) => count + 1)
  }

  return (
    <div className="site-shell" id="top">
      <a className="skip-link" href="#main-content">
        Skip to main content
      </a>

      <header className="site-header">
        <div className="header-content">
          <a className="brand" href="#top" aria-label="ServiceFlow home">
            <span className="brand-mark" aria-hidden="true">SF</span>
            <span>ServiceFlow</span>
          </a>
          <span className="section-label">Customer services</span>
        </div>
      </header>

      <main id="main-content">
        <section className="intro" aria-labelledby="intro-heading">
          <p className="eyebrow">Reliable service starts here</p>
          <h1 id="intro-heading">Straightforward help for your property</h1>
          <p className="intro-copy">
            Browse available services, compare starting prices, and find the
            right option before requesting a personalized estimate.
          </p>
        </section>

        <section className="services-section" aria-labelledby="services-heading">
          <div className="section-heading">
            <div>
              <p className="eyebrow">Service catalog</p>
              <h2 id="services-heading">Available services</h2>
            </div>
            {!loading && !error && services.length > 0 && (
              <p className="service-count">
                {services.length} {services.length === 1 ? 'service' : 'services'}
              </p>
            )}
          </div>

          {loading && (
            <div className="state-panel" role="status" aria-live="polite">
              <span className="state-indicator" aria-hidden="true" />
              <div>
                <h3>Loading services</h3>
                <p>We’re retrieving the latest service details.</p>
              </div>
            </div>
          )}

          {!loading && error && (
            <div className="state-panel state-panel-error" role="alert">
              <div>
                <h3>Services are temporarily unavailable</h3>
                <p>{error} Please try again.</p>
              </div>
              <button type="button" onClick={retryRequest}>
                Try again
              </button>
            </div>
          )}

          {!loading && !error && services.length === 0 && (
            <div className="state-panel" role="status">
              <div>
                <h3>No services are available right now</h3>
                <p>Please check back soon for updated offerings.</p>
              </div>
            </div>
          )}

          {!loading && !error && services.length > 0 && (
            <div className="service-grid">
              {services.map((service) => (
                <article className="service-card" key={service.id}>
                  <div className="card-accent" aria-hidden="true" />
                  <div className="card-content">
                    <h3>{service.name}</h3>
                    <p className="service-description">{service.description}</p>
                    <dl className="service-details">
                      <div>
                        <dt>Starting price</dt>
                        <dd>{currencyFormatter.format(service.startingPrice)}</dd>
                      </div>
                      <div>
                        <dt>Estimated duration</dt>
                        <dd>{service.estimatedMinutes} minutes</dd>
                      </div>
                    </dl>
                  </div>
                </article>
              ))}
            </div>
          )}
        </section>
      </main>

      <footer className="site-footer">
        <p>ServiceFlow</p>
        <p>Clear services. Straightforward pricing.</p>
      </footer>
    </div>
  )
}

export default App
